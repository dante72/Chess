USE [Chess]
GO

INSERT INTO [dbo].[Exercises]
           ([Name]
           ,[Value]
           ,[Moves])
     VALUES
           ('Task1', 'b3WB b5WN b6WK d5WN e5BP e6BK d7BP f7BP h7WQ',2),
		   ('Task2', 'a3WP a4BP c5BK c6BP d8WB d2WR e2WQ e4WK f4WP',2),
		   ('Task3', 'a7WB d2WR g7WN g2BQ g1WN h1BB h2BK h4BP g7WR h8WK',2),
           ('Task4', 'a8BR a7BP b7BP c7BP c8BB d8BK d6BP f7BP g7BP h7BP h8BR g8BN a5BB b5WQ f5BQ d4BP a3WB a2WP a1WR b1WN c3WP d1WK e1WR e2WB f2WP g2WP h2WP',3)
GO
