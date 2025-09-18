using CampusConnect.Data;
using CampusConnect.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CampusConnect.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // IMPROVEMENT: More efficient. Now only calls SaveChanges() once.
        public Question CreateQuestionWithTags(Question question, string tagsString)
        {
            var tagNames = tagsString.Split(',')
                                     .Select(t => t.Trim().ToLower())
                                     .Where(t => !string.IsNullOrEmpty(t))
                                     .Distinct();

            foreach (var tagName in tagNames)
            {
                var existingTag = _context.Tags.FirstOrDefault(t => t.Name == tagName);

                if (existingTag == null)
                {
                    existingTag = new Tag { Name = tagName };
                    _context.Tags.Add(existingTag); // Add new tag to the context, but don't save yet
                }

                // Associate the tag with the question
                question.QuestionTags.Add(new QuestionTag { Tag = existingTag });
            }

            _context.Questions.Add(question);
            _context.SaveChanges(); // A single save operation handles everything
            return question;
        }

        // No changes needed, this was correct.
        public List<Question> GetAllWithUsers()
        {
            return _context.Questions
                           .Include(q => q.ApplicationUser)
                           .OrderByDescending(q => q.CreatedAt)
                           .ToList();
        }

        // FIX: Changed return type to Question? to handle nulls correctly.
        public Question? GetById(int id)
        {
            return _context.Questions
                .Include(q => q.ApplicationUser)
                .Include(q => q.Answers)
                    .ThenInclude(a => a.ApplicationUser)
                .Include(q => q.QuestionTags)
                    .ThenInclude(qt => qt.Tag)
                .FirstOrDefault(q => q.Id == id);
        }

        // MAJOR FIX: The original Update method did not handle tag changes. This one does.
        public void Update(Question question, string tagsString)
        {
            var originalQuestion = _context.Questions
                .Include(q => q.QuestionTags)
                .ThenInclude(qt => qt.Tag)
                .FirstOrDefault(q => q.Id == question.Id);

            if (originalQuestion == null) return;

            // Update simple properties
            originalQuestion.Title = question.Title;
            originalQuestion.Content = question.Content;

            // --- Tag Update Logic ---
            var newTagNames = tagsString.Split(',')
                .Select(t => t.Trim().ToLower())
                .Where(t => !string.IsNullOrEmpty(t))
                .ToHashSet();

            var currentTagNames = originalQuestion.QuestionTags
                .Select(qt => qt.Tag.Name)
                .ToHashSet();

            // Tags to remove
            var tagsToRemove = originalQuestion.QuestionTags
                .Where(qt => !newTagNames.Contains(qt.Tag.Name))
                .ToList();
            _context.QuestionTags.RemoveRange(tagsToRemove);

            // Tags to add
            var tagNamesToAdd = newTagNames.Where(n => !currentTagNames.Contains(n));
            foreach (var tagName in tagNamesToAdd)
            {
                var existingTag = _context.Tags.FirstOrDefault(t => t.Name == tagName);
                if (existingTag == null)
                {
                    existingTag = new Tag { Name = tagName };
                    _context.Tags.Add(existingTag);

                }
                originalQuestion.QuestionTags.Add(new QuestionTag { Tag = existingTag });
            }

            _context.SaveChanges();
        }

        // IMPROVEMENT: More efficient. No need to load all related data just to delete.
        public void Delete(int id)
        {
            var question = _context.Questions.Find(id); // Find is more efficient than GetById here.
            if (question != null)
            {
                _context.Questions.Remove(question);
                _context.SaveChanges();
            }
        }
    }
}