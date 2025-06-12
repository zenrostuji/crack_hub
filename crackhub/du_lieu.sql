-- GameLinks table data - Fixed to match GameLink model
-- Model properties: Id, GameId, LinkName (max 100), Url (required), FileSize (max 50), PartNumber, TotalParts, Password (max 100), IsActive, DateAdded (datetime)

SET IDENTITY_INSERT [dbo].[GameLinks] ON 

-- Game 1 - The Witcher 3: Wild Hunt
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (1, 1, N'Google Drive', N'https://drive.google.com/file/d/1abc123/view', N'25 GB', 1, 2, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (2, 1, N'Google Drive', N'https://drive.google.com/file/d/2def456/view', N'25 GB', 2, 2, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (3, 1, N'MediaFire', N'https://mediafire.com/file/witcher3_part1', N'25 GB', 1, 2, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (4, 1, N'MediaFire', N'https://mediafire.com/file/witcher3_part2', N'25 GB', 2, 2, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))

-- Game 2 - Grand Theft Auto V
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (5, 2, N'Google Drive', N'https://drive.google.com/file/d/3ghi789/view', N'35 GB', 1, 2, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (6, 2, N'Google Drive', N'https://drive.google.com/file/d/4jkl012/view', N'37 GB', 2, 2, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (7, 2, N'MEGA', N'https://mega.nz/file/gta5_part1', N'35 GB', 1, 2, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (8, 2, N'MEGA', N'https://mega.nz/file/gta5_part2', N'37 GB', 2, 2, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))

-- Game 3 - Cyberpunk 2077
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (9, 3, N'Google Drive', N'https://drive.google.com/file/d/5mno345/view', N'34 GB', 1, 2, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (10, 3, N'Google Drive', N'https://drive.google.com/file/d/6pqr678/view', N'36 GB', 2, 2, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (11, 3, N'MEGA', N'https://mega.nz/file/cyberpunk_part1', N'34 GB', 1, 2, N'cp2077', 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (12, 3, N'MEGA', N'https://mega.nz/file/cyberpunk_part2', N'36 GB', 2, 2, N'cp2077', 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))

-- Game 4 - FIFA 23
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (13, 4, N'Google Drive', N'https://drive.google.com/file/d/7stu901/view', N'50 GB', 1, 1, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (14, 4, N'MEGA', N'https://mega.nz/file/fifa23_full', N'50 GB', 1, 1, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (15, 4, N'MediaFire', N'https://mediafire.com/file/fifa23_full', N'50 GB', 1, 1, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))

-- Game 5 - Minecraft
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (16, 5, N'Google Drive', N'https://drive.google.com/file/d/8vwx234/view', N'250 MB', 1, 1, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (17, 5, N'MediaFire', N'https://mediafire.com/file/minecraft_launcher', N'250 MB', 1, 1, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))

-- Game 6
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (18, 6, N'PixelDrain', N'https://pixeldrain.com/u/SeZ9mSnW', N'645 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 7
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (19, 7, N'PixelDrain', N'https://pixeldrain.com/u/ds3qKj57', N'1.72 GB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 8 (missing data - added)
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (20, 8, N'MEGA', N'https://mega.nz/file/game8_link', N'15 GB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 9 (missing data - added)
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (21, 9, N'Google Drive', N'https://drive.google.com/file/d/game9_link/view', N'8 GB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 10
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (22, 10, N'GoFile', N'https://gofile.io/d/5HX4ea', N'6 GB', 1, 2, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 11
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (23, 11, N'PixelDrain', N'https://pixeldrain.com/u/v5PWQ8H5', N'340 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 12
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (24, 12, N'PixelDrain', N'https://pixeldrain.com/u/FrQC6Utv', N'12.1 GB', 1, 2, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 13
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (25, 13, N'PixelDrain', N'https://pixeldrain.com/u/21G5wNyN', N'350 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 14
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (26, 14, N'PixelDrain', N'https://pixeldrain.com/u/xNs3zBpK', N'570 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 15
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (27, 15, N'PixelDrain', N'https://pixeldrain.com/u/7VoLE3oM', N'2 GB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (28, 15, N'MediaFire', N'https://mediafire.com/file/findlove_full', N'2 GB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 16
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (29, 16, N'MEGA', N'https://mega.nz/file/0StHnSqQ#8Q8dWb3FArjXdFPZ35QETF8jAKWVg1Tf0X1S_hPlHQA', N'420 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 17
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (30, 17, N'PixelDrain', N'https://pixeldrain.com/u/4AhVkV8A', N'9.2 GB', 1, 2, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 18
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (31, 18, N'MixDrop', N'https://mixdrop.sb/f/xodkddg1uld1wnm', N'1 GB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 19
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (32, 19, N'PixelDrain', N'https://pixeldrain.com/u/aZa1kZSc', N'2 GB', 1, 2, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 20
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (33, 20, N'MEGA', N'https://mega.nz/file/u4AhyABC#R70uIoiuzUamyWRpUdsm7D4zgfUozsToTGeZfbI3F_w', N'140 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 21
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (34, 21, N'PixelDrain', N'https://pixeldrain.com/u/VmEqSA4d', N'2.4 GB', 1, 2, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 22
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (35, 22, N'PixelDrain', N'https://pixeldrain.com/u/B8Mkw9yX', N'1.3 GB', 1, 2, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 23
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (36, 23, N'PixelDrain', N'https://pixeldrain.com/u/qoMPwV61', N'9 GB', 1, 2, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 24
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (37, 24, N'PixelDrain', N'https://pixeldrain.com/u/z9byGKqx', N'255 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 25
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (38, 25, N'Steam', N'https://store.steampowered.com/app/1289310/Helltaker/', N'350 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 26
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (39, 26, N'PixelDrain', N'https://pixeldrain.com/u/NvULjVXy', N'7 GB', 1, 2, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 27
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (40, 27, N'MEGA', N'https://mega.nz/file/AbslCBZT#9XiUx0cCMtwqfX6Kn2BzKEIewElY_ouR8unvV1hgDaE', N'152 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 28
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (41, 28, N'PixelDrain', N'https://pixeldrain.com/u/mqaduXew', N'5.8 GB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 29
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (42, 29, N'MEGA', N'https://mega.nz/file/MBhWyayC#Fg1JgZkxYQAWBl9uVCFivI_0pVTyVO9IG0EaOoIR0E8', N'305 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 30
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (43, 30, N'Google Drive', N'https://drive.google.com/file/d/1Y7eTgkSV8NDABKxehos2Lm7_p0ont6fq/view', N'341 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 31
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (44, 31, N'MEGA', N'https://mega.nz/file/5qAGTCgC#140dx-b1E8qV3mh3Rb14snj-ZzxwyLXpvU6GgtShQOE', N'360 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 32
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (45, 32, N'PixelDrain', N'https://pixeldrain.com/u/JMhqHy18', N'262 MB', 1, 2, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (46, 32, N'MEGA', N'https://mega.nz/file/jIUwXIZQ#i3Z8Rn-Z79JTCjTDHH72GqIqm2wH8gEkODQDmtBu7TA', N'14 MB', 1, 1, N'sg0003', 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 33
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (47, 33, N'PixelDrain', N'https://pixeldrain.com/u/KMDVWjXV', N'8.3 GB', 1, 2, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 34
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (48, 34, N'MEGA', N'https://mega.nz/file/5mRx1aIJ#Av8iC5YOFRrjc-Mldpaal2GERE7-A6CEajzmtRpn2Zw', N'10 GB', 1, 1, N'larry456', 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 35
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (49, 35, N'PixelDrain', N'https://pixeldrain.com/u/2eScxKLN', N'1.4 GB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 36
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (50, 36, N'PixelDrain', N'https://pixeldrain.com/u/A5PeKEbG', N'333 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 37
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (51, 37, N'PixelDrain', N'https://pixeldrain.com/u/ezKWZfCP', N'1.5 GB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))

-- Game 38
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (52, 38, N'TeraBox', N'https://dm.terabox.com/vietnamese/sharing/link?surl=iGrn4LI711z1baxz9wMCtQ&clearCache=1', N'73 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (54, 38, N'Google Drive', N'https://drive.google.com/file/d/1jv0gtLVChifBLuGGSDADc8QY1uEKgFKt/view', N'500 MB', 1, 1, NULL, 1, CAST(N'2025-04-30T16:46:57.083' AS DateTime))

SET IDENTITY_INSERT [dbo].[GameLinks] OFF