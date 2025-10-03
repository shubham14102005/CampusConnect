-- Fix Vote Counts Script
-- This script recalculates vote counts for all answers based on existing votes

UPDATE Answers
SET 
    UpvoteCount = (
        SELECT COALESCE(SUM(
            CASE 
                WHEN av.IsUpVote = 1 AND u.UserName LIKE '%@faculty.ddu.ac.in' THEN 10
                WHEN av.IsUpVote = 1 THEN 1
                ELSE 0
            END
        ), 0)
        FROM AnswerVotes av
        INNER JOIN AspNetUsers u ON av.ApplicationUserId = u.Id
        WHERE av.AnswerId = Answers.Id
    ),
    DownvoteCount = (
        SELECT COALESCE(SUM(
            CASE 
                WHEN av.IsUpVote = 0 AND u.UserName LIKE '%@faculty.ddu.ac.in' THEN 10
                WHEN av.IsUpVote = 0 THEN 1
                ELSE 0
            END
        ), 0)
        FROM AnswerVotes av
        INNER JOIN AspNetUsers u ON av.ApplicationUserId = u.Id
        WHERE av.AnswerId = Answers.Id
    );

-- Display results
SELECT 
    a.Id as AnswerId,
    a.Content,
    a.UpvoteCount,
    a.DownvoteCount,
    COUNT(av.Id) as TotalVotes
FROM Answers a
LEFT JOIN AnswerVotes av ON a.Id = av.AnswerId
GROUP BY a.Id, a.Content, a.UpvoteCount, a.DownvoteCount
ORDER BY a.Id;