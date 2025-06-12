-- File SQL đã được chỉnh sửa theo đúng model Game.cs
-- Bao gồm tất cả các trường: Id, Title, ShortDescription, FullDescription, CoverImageUrl, Developer, Publisher, ReleaseDate, CategoryId, Rating, Downloads, DownloadUrl, Size, AverageRating, IsFeatured, CreatedDate

SET IDENTITY_INSERT [dbo].[Games] ON 

INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating], [IsFeatured], [CreatedDate]) VALUES 
(1, N'The Witcher 3: Wild Hunt', N'Một trò chơi nhập vai thế giới mở hoành tráng với cốt truyện phong phú và thế giới rộng lớn để khám phá.', N'The Witcher 3: Wild Hunt là một trò chơi nhập vai thế giới mở năm 2015 được phát triển bởi CD Projekt Red. Lấy bối cảnh trong một thế giới giả tưởng dựa trên thần thoại Slav, người chơi điều khiển Geralt xứ Rivia, một thợ săn quái vật được gọi là "witcher" phải tìm con gái nuôi của anh ta, Ciri, đang bị Wildchase truy đuổi, một lực lượng ma quái bí ẩn đe dọa thế giới.', N'/img/games/thewitcher3.jpg', N'CD Projekt Red', N'CD Projekt', CAST(N'2015-05-19T00:00:00.000' AS DateTime), 2, 9.5, N'1.2M', NULL, N'50 GB', 9.3, 1, CAST(N'2024-01-01T00:00:00.000' AS DateTime)),

(2, N'Grand Theft Auto V', N'Một trò chơi hành động phiêu lưu thế giới mở, diễn ra tại thành phố hư cấu Los Santos.', N'Grand Theft Auto V là một tựa game hành động-phiêu lưu năm 2013 được phát triển bởi Rockstar North và được xuất bản bởi Rockstar Games. Là phần chính thứ bảy trong series Grand Theft Auto, trò chơi có ba nhân vật chính: tên tội phạm đã về hưu Michael De Santa, gangster đường phố Franklin Clinton và gã buôn ma túy Trevor Philips, kể về việc họ thực hiện các vụ cướp dưới áp lực của một cơ quan chính phủ tham nhũng và những tên tội phạm mạnh mẽ.', N'/img/games/gta5.jpg', N'Rockstar North', N'Rockstar Games', CAST(N'2013-09-17T00:00:00.000' AS DateTime), 1, 9.6, N'2.1M', NULL, N'72 GB', 9.5, 1, CAST(N'2024-01-02T00:00:00.000' AS DateTime)),

(3, N'Cyberpunk 2077', N'Một trò chơi nhập vai hành động thế giới mở lấy bối cảnh trong một thành phố đầy rẫy tội phạm và rất nhiều công nghệ cao.', N'Cyberpunk 2077 là một trò chơi nhập vai hành động thế giới mở lấy bối cảnh ở Night City, một đô thị cyberpunk nơi quyền lực, sự xa hoa, và những phẫu thuật cơ thể đang là những thứ được săn đón. Bạn đóng vai V, một lính đánh thuê ngoài vòng pháp luật đang theo đuổi một bộ phận cấy ghép độc nhất vô nhị là chìa khóa dẫn đến sự bất tử.', N'/img/games/cyberpunk2077.jpg', N'CD Projekt Red', N'CD Projekt', CAST(N'2020-12-10T00:00:00.000' AS DateTime), 1, 8.7, N'1.8M', NULL, N'70 GB', 7.9, 1, CAST(N'2024-01-03T00:00:00.000' AS DateTime)),

(4, N'FIFA 23', N'Trò chơi bóng đá mô phỏng mới nhất từ EA Sports với các giải đấu, câu lạc bộ và cầu thủ chính thức.', N'FIFA 23 là một trò chơi mô phỏng bóng đá được phát triển bởi EA Vancouver và EA Romania và được xuất bản bởi EA Sports. Đây là phần thứ 30 và cũng là phần cuối cùng trong loạt trò chơi FIFA do EA phát triển. Được hỗ trợ bởi công nghệ HyperMotion2, FIFA 23 mang đến trải nghiệm bóng đá thực tế hơn với các hoạt ảnh cầu thủ chân thực được xây dựng từ dữ liệu ghi nhận từ các trận đấu thực tế.', N'/img/games/fifa23.jpg', N'EA Vancouver', N'EA Sports', CAST(N'2022-09-30T00:00:00.000' AS DateTime), 5, 8.5, N'950K', NULL, N'50 GB', 8.2, 0, CAST(N'2024-01-04T00:00:00.000' AS DateTime)),

(5, N'Minecraft', N'Một trò chơi sandbox cho phép người chơi khám phá, xây dựng và sinh tồn trong một thế giới khối hình học.', N'Minecraft là một trò chơi sandbox được phát triển bởi Mojang Studios. Trò chơi được tạo ra bởi Markus "Notch" Persson trong ngôn ngữ lập trình Java. Trong Minecraft, người chơi khám phá một thế giới block 3D được tạo ra theo cách khởi tạo thủ tục, khám phá và khai thác nguyên liệu thô, chế tạo các công cụ và vật phẩm, xây dựng các công trình, và tùy chọn có thể chiến đấu với quái vật máy tính.', N'/img/games/minecraft.jpg', N'Mojang Studios', N'Mojang Studios', CAST(N'2011-11-18T00:00:00.000' AS DateTime), 15, 9.2, N'1.5M', NULL, N'2 GB', 9.4, 1, CAST(N'2024-01-05T00:00:00.000' AS DateTime)),

(6, N'Fear & Hunger', N'Game RPG kinh dị tâm lý với gameplay hardcore và cơ chế sinh tồn khắc nghiệt.', N'Fear & Hunger là một trò chơi nhập vai kinh dị tâm lý độc lập. Game lấy bối cảnh trong một hầm ngục cổ đại, nơi người chơi phải đối mặt với những sinh vật kinh dị và cạm bẫy chết người. Với cơ chế permadeath và hệ thống chiến đấu khắc nghiệt, game mang đến trải nghiệm kinh dị và đầy thách thức.', N'/img/games/fear_and_hunger.jpg', N'Miro Haverinen', N'Miro Haverinen', CAST(N'2018-10-30T00:00:00.000' AS DateTime), 8, 8.7, N'420K', NULL, N'500 MB', 8.5, 0, CAST(N'2024-01-15T00:00:00.000' AS DateTime)),

(7, N'Fear & Hunger 2', N'Phần tiếp theo của series RPG kinh dị với những cải tiến về đồ họa và gameplay.', N'Fear & Hunger 2 là phần tiếp theo của tựa game kinh dị cult classic. Game mang đến một thế giới mới rộng lớn hơn với nhiều địa điểm để khám phá, các sinh vật kinh dị mới và cốt truyện sâu sắc hơn. Vẫn giữ nguyên tinh thần hardcore và bầu không khí ám ảnh của phần đầu.', N'/img/games/fear_and_hunger_2.jpg', N'Miro Haverinen', N'Miro Haverinen', CAST(N'2021-12-24T00:00:00.000' AS DateTime), 8, 8.9, N'380K', NULL, N'800 MB', 8.7, 0, CAST(N'2024-01-16T00:00:00.000' AS DateTime)),

(8, N'Lona RPG', N'Game nhập vai màu mè với yếu tố chiến đấu theo lượt và cốt truyện độc đáo.', N'Lona RPG là game nhập vai phong cách anime với hệ thống chiến đấu theo lượt. Game kể về cuộc phiêu lưu của Lona, một cô gái trẻ với khả năng đặc biệt, trong hành trình khám phá thế giới và tìm hiểu về quá khứ của mình. Với đồ họa sặc sỡ và hệ thống nhân vật phong phú.', N'/img/games/lona_rpg.jpg', N'Gossamer Games', N'Gossamer Games', CAST(N'2020-06-15T00:00:00.000' AS DateTime), 2, 7.8, N'250K', NULL, N'2 GB', 7.5, 0, CAST(N'2024-01-17T00:00:00.000' AS DateTime)),

(9, N'SiNiSistar2', N'Game kinh dị tâm lý với đồ họa pixel art và câu chuyện ám ảnh.', N'SiNiSistar2 là một trò chơi kinh dị indie theo phong cách pixel art. Game đưa người chơi vào vai một nhân vật bị mắc kẹt trong một thế giới kì lạ và phải tìm cách thoát ra trong khi đối mặt với những thực thể kinh dị. Nổi bật với âm nhạc ám ảnh và cốt truyện rối ren, phi tuyến tính.', N'/img/games/sinisistar2.jpg', N'Remote Control Studio', N'Remote Control Studio', CAST(N'2021-10-31T00:00:00.000' AS DateTime), 8, 8.2, N'180K', NULL, N'1 GB', 8.0, 0, CAST(N'2024-01-18T00:00:00.000' AS DateTime)),

(10, N'嗜血印 Bloody Spell', N'Game hành động võ hiệp Trung Quốc với hệ thống chiến đấu đẹp mắt và chặt chém đã tay.', N'Bloody Spell là một game hành động võ hiệp phong cách Trung Quốc với hệ thống chiến đấu nhanh, mãn nhãn. Game lấy bối cảnh thế giới cổ đại nơi người chơi vào vai một môn đồ võ lâm trên hành trình tìm kiếm báo thù và khám phá bí ẩn về "Bloody Spell" - một bùa chú cổ xưa đầy quyền năng.', N'/img/games/bloody_spell.jpg', N'Yooreka Studio', N'Yooreka Studio', CAST(N'2019-01-16T00:00:00.000' AS DateTime), 1, 7.9, N'420K', NULL, N'12 GB', 7.6, 0, CAST(N'2024-01-19T00:00:00.000' AS DateTime)),

(11, N'Boo Party', N'Game phiêu lưu hành động vui nhộn với nhân vật chính là những con ma đáng yêu.', N'Boo Party là một trò chơi phiêu lưu hành động đáng yêu với các nhân vật ma dễ thương. Người chơi sẽ điều khiển những con ma với khả năng đặc biệt khác nhau để giải các câu đố, vượt qua chướng ngại vật và khám phá thế giới đầy màu sắc. Game phù hợp với mọi lứa tuổi.', N'/img/games/boo_party.jpg', N'Ghost Games Studio', N'Playful Entertainment', CAST(N'2022-04-15T00:00:00.000' AS DateTime), 3, 7.5, N'120K', NULL, N'4 GB', 7.2, 0, CAST(N'2024-01-20T00:00:00.000' AS DateTime)),

(12, N'Steins;Gate', N'Visual novel khoa học viễn tưởng nổi tiếng về du hành thời gian với cốt truyện sâu sắc.', N'Steins;Gate là một visual novel khoa học viễn tưởng kinh điển kể về nhóm bạn trẻ tình cờ phát minh ra máy du hành thời gian và những hậu quả không lường trước được. Với cốt truyện phức tạp, nhân vật được xây dựng chặt chẽ và nhiều nhánh kết thúc khác nhau, Steins;Gate được coi là một trong những visual novel hay nhất mọi thời đại.', N'/img/games/steins_gate.jpg', N'5pb.', N'Spike Chunsoft', CAST(N'2009-10-15T00:00:00.000' AS DateTime), 16, 9.4, N'1.2M', NULL, N'6 GB', 9.2, 1, CAST(N'2024-01-21T00:00:00.000' AS DateTime)),

(13, N'Enthralled', N'Visual novel kinh dị tâm lý với nhiều lựa chọn ảnh hưởng đến cốt truyện.', N'Enthralled là một visual novel kinh dị tâm lý với những lựa chọn có ảnh hưởng sâu rộng đến diễn biến cốt truyện. Game lấy bối cảnh một thị trấn nhỏ bị ám ảnh bởi những hiện tượng siêu nhiên, nơi người chơi phải làm sáng tỏ những bí mật đen tối trong khi cố gắng giữ mạng sống của mình.', N'/img/games/enthralled.jpg', N'Dark Tale Studios', N'Sekai Project', CAST(N'2021-07-23T00:00:00.000' AS DateTime), 8, 8.0, N'190K', NULL, N'3 GB', 7.8, 0, CAST(N'2024-01-22T00:00:00.000' AS DateTime)),

(14, N'Occult Rewrite', N'Visual novel ma thuật với phong cách anime và nhiều nhánh cốt truyện.', N'Occult Rewrite là một visual novel về ma thuật và thế giới siêu nhiên. Người chọi vào vai một học viên ma thuật với khả năng "viết lại" thực tại, phải sử dụng sức mạnh này để ngăn chặn một thảm họa sắp xảy ra. Game nổi bật với artwork đẹp mắt, hệ thống nhân vật đa dạng và nhiều nhánh cốt truyện khác nhau.', N'/img/games/occult_rewrite.jpg', N'Mystery Circle', N'Moonlight Games', CAST(N'2022-01-10T00:00:00.000' AS DateTime), 16, 8.3, N'210K', NULL, N'5 GB', 8.1, 0, CAST(N'2024-01-23T00:00:00.000' AS DateTime)),

(15, N'Find Love or Die Trying', N'Dating sim với yếu tố kinh dị và nhiều twist bất ngờ trong cốt truyện.', N'Find Love or Die Trying là một trò chơi hẹn hò với twist kinh dị, nơi người chơi không chỉ tìm kiếm tình yêu mà còn phải sống sót. Bên dưới vẻ ngoài của một dating sim thông thường là những bí mật đen tối và những tình tiết rùng rợn. Mỗi lựa chọn đều có thể dẫn đến tình yêu hoặc cái chết.', N'/img/games/find_love_or_die.jpg', N'Twisted Date Studios', N'Horror Dating Inc', CAST(N'2021-02-14T00:00:00.000' AS DateTime), 8, 7.7, N'150K', NULL, N'2 GB', 7.5, 0, CAST(N'2024-01-24T00:00:00.000' AS DateTime)),

(16, N'Sable''s Grimoire', N'Visual novel fantasy về một học viện ma thuật với các nhân vật là sinh vật huyền bí.', N'Sable''s Grimoire là một visual novel fantasy lấy bối cảnh tại một học viện ma thuật, nơi con người học tập cùng các sinh vật huyền bí như rồng, tiên, người sói. Người chơi vào vai Sable, một học viên ma thuật con người, trong hành trình học tập và xây dựng mối quan hệ với các bạn học kỳ lạ của mình.', N'/img/games/sables_grimoire.jpg', N'Zetsubou Games', N'Sekai Project', CAST(N'2018-01-30T00:00:00.000' AS DateTime), 16, 8.2, N'280K', NULL, N'2 GB', 8.0, 0, CAST(N'2024-01-25T00:00:00.000' AS DateTime)),

(17, N'Tokyo Babel', N'Visual novel kịch tính về cuộc chiến giữa thiên đường và địa ngục.', N'Tokyo Babel là một visual novel kịch tính với bối cảnh ngày tận thế, nơi thiên đường và địa ngục đã bị phong ấn và các linh hồn tụ họp tại Tokyo Babel - một tháp Babel mới. Người chơi sẽ theo dõi câu chuyện của nhiều nhân vật với những động cơ khác nhau trong một cuộc chiến sinh tồn khốc liệt.', N'/img/games/tokyo_babel.jpg', N'PropellerTeam', N'MangaGamer', CAST(N'2011-05-27T00:00:00.000' AS DateTime), 16, 8.5, N'320K', NULL, N'4 GB', 8.3, 0, CAST(N'2024-01-26T00:00:00.000' AS DateTime)),

(18, N'My Lovely Wife', N'Mô phỏng quản lý hẹn hò độc đáo với yếu tố ma thuật đen tối và đạo đức xám.', N'My Lovely Wife là một trò chơi mô phỏng quản lý độc đáo kết hợp với yếu tố hẹn hò và ma thuật đen tối. Người chơi vào vai một người đàn ông tìm cách hồi sinh người vợ đã mất bằng cách triệu hồi và "hẹn hò" với các ác quỷ. Game thách thức người chơi với những lựa chọn đạo đức xám và kết quả không lường trước.', N'/img/games/my_lovely_wife.jpg', N'GameChanger Studio', N'Toge Productions', CAST(N'2021-08-10T00:00:00.000' AS DateTime), 6, 7.9, N'150K', NULL, N'1 GB', 7.6, 0, CAST(N'2024-01-27T00:00:00.000' AS DateTime)),

(19, N'The Letter - Horror Visual Novel', N'Visual novel kinh dị tâm lý về một biệt thự bị nguyền rủa với bảy nhân vật chính.', N'The Letter là một visual novel kinh dị tâm lý kể về bảy cá nhân liên quan đến một biệt thự cổ bị nguyền rủa. Sau khi nhận được một lá thư bí ẩn, họ bị cuốn vào một chuỗi sự kiện kinh hoàng. Game nổi bật với đồ họa đẹp mắt, âm thanh rùng rợn và cốt truyện đan xen giữa nhiều nhân vật với những góc nhìn khác nhau.', N'/img/games/the_letter.jpg', N'Yangyang Mobile', N'Yangyang Mobile', CAST(N'2017-07-24T00:00:00.000' AS DateTime), 8, 9.0, N'480K', NULL, N'7 GB', 8.8, 1, CAST(N'2024-01-28T00:00:00.000' AS DateTime)),

(20, N'Friday Night Funkin'' Remake', N'Phiên bản làm lại của trò chơi nhịp điệu độc lập nổi tiếng với nhiều bài hát và nhân vật mới.', N'Friday Night Funkin'' Remake là phiên bản nâng cấp của trò chơi nhịp điệu độc lập nổi tiếng. Game kể về một chàng trai phải đấu rap với nhiều đối thủ khác nhau để giành được trái tim của bạn gái. Phiên bản remake bổ sung thêm nhiều bài hát, nhân vật mới và cải thiện đáng kể về mặt đồ họa và gameplay.', N'/img/games/fnf_remake.jpg', N'Funkin'' Crew', N'Funkin'' Crew', CAST(N'2022-03-15T00:00:00.000' AS DateTime), 7, 8.5, N'1.5M', NULL, N'500 MB', 8.3, 0, CAST(N'2024-01-29T00:00:00.000' AS DateTime)),

(21, N'Utawarerumono: Prelude to the Fallen + All DLC', N'Visual novel chiến thuật với cốt truyện sâu sắc về một thế giới fantasy đầy bí ẩn.', N'Utawarerumono: Prelude to the Fallen là phần đầu tiên trong series visual novel kết hợp với game chiến thuật theo lượt. Game kể về một người đàn ông mất trí nhớ được cứu bởi một cô gái trẻ trong một ngôi làng nhỏ. Khi làng bị tấn công, anh buộc phải dẫn dắt dân làng chống lại kẻ thù và dần khám phá ra bí mật về bản thân và thế giới xung quanh.', N'/img/games/utawarerumono_prelude.jpg', N'Aquaplus', N'Shiravune', CAST(N'2020-05-26T00:00:00.000' AS DateTime), 16, 8.7, N'280K', NULL, N'10 GB', 8.5, 0, CAST(N'2024-01-30T00:00:00.000' AS DateTime)),

(22, N'Leisure Suit Larry - Wet Dreams Dry Twice', N'Game phiêu lưu hài hước người lớn với nhân vật chính là Larry Laffer cùng các tình huống hài hước.', N'Leisure Suit Larry - Wet Dreams Dry Twice là phần tiếp theo của series phiêu lưu hài hước dành cho người lớn. Game tiếp tục câu chuyện của Larry Laffer trong hành trình tìm kiếm tình yêu đích thực của mình. Với đồ họa hoạt hình sặc sỡ, các câu đố thông minh và rất nhiều tình huống hài hước, game mang đến nhiều tiếng cười cho người chơi.', N'/img/games/larry_wet_dreams.jpg', N'CrazyBunch', N'Assemble Entertainment', CAST(N'2020-10-23T00:00:00.000' AS DateTime), 3, 7.5, N'220K', NULL, N'8 GB', 7.3, 0, CAST(N'2024-01-31T00:00:00.000' AS DateTime)),

(23, N'Death end re;Quest 2', N'JRPG kinh dị với hệ thống chiến đấu độc đáo và cốt truyện ám ảnh.', N'Death end re;Quest 2 là một JRPG kết hợp với yếu tố kinh dị, kể về Mai Toyama - một cô gái trẻ đến trường nội trú Wordsworth để tìm kiếm người chị mất tích. Nhưng đằng sau vẻ ngoài yên bình của trường học là những bí mật đen tối và sinh vật kinh dị xuất hiện vào ban đêm. Game nổi bật với hệ thống chiến đấu kết hợp turn-based và action, cùng cốt truyện nhiều twist bất ngờ.', N'/img/games/death_end_request2.jpg', N'Compile Heart', N'Idea Factory', CAST(N'2020-08-18T00:00:00.000' AS DateTime), 2, 8.2, N'190K', NULL, N'15 GB', 8.0, 0, CAST(N'2024-02-01T00:00:00.000' AS DateTime)),

(24, N'SPACE-FRIGHT', N'Game kinh dị sinh tồn trong không gian với yếu tố stealth và nỗi sợ cô đơn.', N'SPACE-FRIGHT là một trò chơi kinh dị sinh tồn lấy bối cảnh trên một trạm không gian bị bỏ hoang. Người chơi vào vai một phi hành gia cô đơn phải tìm cách sống sót và thoát khỏi trạm không gian trong khi bị săn đuổi bởi một thực thể bí ẩn. Game tạo nên không khí căng thẳng thông qua yếu tố stealth và âm thanh đáng sợ.', N'/img/games/space_fright.jpg', N'Cosmic Horror Games', N'Deep Space Studios', CAST(N'2021-10-31T00:00:00.000' AS DateTime), 8, 8.0, N'85K', NULL, N'5 GB', 7.7, 0, CAST(N'2024-02-02T00:00:00.000' AS DateTime)),

(25, N'Helltaker', N'Game giải đố ngắn với phong cách anime và nhiều ác quỷ xinh đẹp.', N'Helltaker là một trò chơi giải đố ngắn và miễn phí với phong cách anime. Người chơi vào vai một người đàn ông quyết tâm xây dựng một hậu cung toàn ác quỷ. Mỗi màn chơi, người chơi phải giải các câu đố dựa trên lưới để tiếp cận và thuyết phục một ác quỷ khác nhau gia nhập hậu cung của mình.', N'/img/games/helltaker.jpg', N'Vanripper', N'Vanripper', CAST(N'2020-05-11T00:00:00.000' AS DateTime), 7, 8.6, N'2.2M', NULL, N'100 MB', 8.7, 1, CAST(N'2024-02-03T00:00:00.000' AS DateTime)),

(26, N'Utawarerumono: Mask of Truth', N'Phần cuối của trilogy visual novel chiến thuật với cốt truyện sâu sắc về chiến tranh và chính trị.', N'Utawarerumono: Mask of Truth là phần cuối cùng của trilogy, tiếp nối trực tiếp sự kiện của Mask of Deception. Game kết hợp giữa visual novel và chiến thuật theo lượt, với cốt truyện tập trung vào cuộc nội chiến và những âm mưu chính trị trong một thế giới fantasy. Người chơi sẽ theo dõi câu chuyện về Haku và những đồng đội trong cuộc chiến sinh tồn và giành lại hòa bình.', N'/img/games/utawarerumono_mot.jpg', N'Aquaplus', N'Atlus', CAST(N'2017-09-05T00:00:00.000' AS DateTime), 16, 9.0, N'260K', NULL, N'12 GB', 8.9, 1, CAST(N'2024-02-04T00:00:00.000' AS DateTime)),

(27, N'Love at First Sight', N'Visual novel lãng mạn về mối tình giữa một chàng trai bình thường và cô gái có một mắt.', N'Love at First Sight là một visual novel lãng mạn kể về mối tình giữa Hiroki và Sachi - một cô gái đeo băng che mắt. Khi tình cảm phát triển, Hiroki dần khám phá ra bí mật đằng sau băng che mắt của Sachi và những khó khăn mà cô phải đối mặt. Một câu chuyện cảm động về việc vượt qua những rào cản về ngoại hình và xã hội.', N'/img/games/love_at_first_sight.jpg', N'Creepy Cute', N'Sekai Project', CAST(N'2015-10-07T00:00:00.000' AS DateTime), 16, 7.8, N'180K', NULL, N'1 GB', 7.5, 0, CAST(N'2024-02-05T00:00:00.000' AS DateTime)),

(28, N'Steins;Gate: My Darling''s Embrace', N'Visual novel lãng mạn trong vũ trụ Steins;Gate với các tuyến tình cảm khác nhau.', N'Steins;Gate: My Darling''s Embrace là một spin-off lãng mạn của series Steins;Gate nổi tiếng. Game tập trung vào các mối quan hệ và tình cảm giữa nhân vật chính Okabe Rintaro với các thành viên nữ trong phòng thí nghiệm. Với nhiều tuyến truyện khác nhau dành cho từng nhân vật, game mang đến một khía cạnh nhẹ nhàng và vui vẻ hơn so với cốt truyện chính.', N'/img/games/steinsgate_darling.jpg', N'5pb.', N'Spike Chunsoft', CAST(N'2019-12-10T00:00:00.000' AS DateTime), 16, 8.5, N'240K', NULL, N'4 GB', 8.2, 0, CAST(N'2024-02-06T00:00:00.000' AS DateTime)),

(29, N'Grotesque Beauty', N'Game kinh dị tâm lý về một họa sĩ bị mắc kẹt trong một gallery tranh biến dạng.', N'Grotesque Beauty là một trò chơi kinh dị tâm lý ngắn về một họa sĩ bị mắc kẹt trong một gallery tranh kỳ lạ. Khi khám phá không gian biến dạng của gallery, người chơi dần khám phá ra sự thật đen tối về chính họa sĩ và những tác phẩm của anh ta. Game nổi bật với phong cách nghệ thuật độc đáo và bầu không khí ám ảnh.', N'/img/games/grotesque_beauty.jpg', N'Distorted Art Games', N'Twisted Visions', CAST(N'2022-02-28T00:00:00.000' AS DateTime), 8, 7.6, N'130K', NULL, N'800 MB', 7.3, 0, CAST(N'2024-02-07T00:00:00.000' AS DateTime)),

(30, N'The Spiral Scouts', N'Game phiêu lưu giải đố với hình ảnh dễ thương nhưng nội dung hài hước dành cho người lớn.', N'The Spiral Scouts là một trò chơi phiêu lưu giải đố với phong cách hình ảnh đáng yêu nhưng nội dung hài hước và đôi khi hơi "đen". Người chơi vào vai Remae, một thành viên của tổ chức Spiral Scouts, phải giải các câu đố phức tạp để kiếm được huy hiệu và khám phá thế giới. Game nổi bật với các câu đố thông minh và lối chơi không theo tuyến tính.', N'/img/games/spiralscouts.jpg', N'Cantaloupe', N'DANG!', CAST(N'2018-09-24T00:00:00.000' AS DateTime), 3, 7.8, N'150K', NULL, N'900 MB', 7.5, 0, CAST(N'2024-02-08T00:00:00.000' AS DateTime)),

(31, N'I Love You, Colonel Sanders! A Finger Lickin'''' Good Dating Simulator', N'Dating sim hài hước lấy bối cảnh trường dạy nấu ăn với nam chính là Colonel Sanders nổi tiếng của KFC.', N'I Love You, Colonel Sanders! A Finger Lickin'''' Good Dating Simulator là một visual novel hẹn hò hài hước chính thức từ thương hiệu KFC. Người chơi vào vai một sinh viên trường nấu ăn, phải chinh phục trái tim của Colonel Sanders trẻ tuổi, đẹp trai. Game nổi bật với phong cách anime sống động, cốt truyện hài hước và những tình huống kỳ quặc đúng phong cách marketing độc đáo của KFC.', N'/img/games/colonel_sanders.jpg', N'Psyop', N'KFC', CAST(N'2019-09-24T00:00:00.000' AS DateTime), 16, 6.8, N'280K', NULL, N'500 MB', 6.5, 0, CAST(N'2024-02-09T00:00:00.000' AS DateTime)),

(32, N'Steins;Gate - Spin Offs', N'Tổng hợp các ngoại truyện của series Steins;Gate nổi tiếng với nhiều câu chuyện mới về các nhân vật quen thuộc.', N'Steins;Gate - Spin Offs là tuyển tập các visual novel ngoại truyện trong vũ trụ Steins;Gate, bao gồm "Steins;Gate: Linear Bounded Phenogram" và "Steins;Gate: Darling of Loving Vows". Các spin-off này mang đến những góc nhìn mới về các nhân vật quen thuộc, với nhiều câu chuyện phụ và kết thúc thay thế không xuất hiện trong cốt truyện chính.', N'/img/games/steinsgate_spinoffs.jpg', N'5pb.', N'Spike Chunsoft', CAST(N'2013-03-14T00:00:00.000' AS DateTime), 16, 8.6, N'190K', NULL, N'5 GB', 8.4, 0, CAST(N'2024-02-10T00:00:00.000' AS DateTime)),

(33, N'Steins;Gate 0', N'Phần tiếp theo của visual novel khoa học viễn tưởng nổi tiếng, khám phá một dòng thời gian thay thế đầy bi kịch.', N'Steins;Gate 0 là phần tiếp theo của visual novel Steins;Gate đình đám, lấy bối cảnh trong một dòng thời gian nơi Okabe Rintarou đã từ bỏ nỗ lực cứu Makise Kurisu và đang phải vật lộn với hội chứng PTSD. Game đưa người chơi vào một câu chuyện tăm tối hơn, với nhiều biến cố và thử thách mới, đồng thời giải thích nhiều bí ẩn còn tồn tại từ phần đầu tiên.', N'/img/games/steinsgate0.jpg', N'5pb.', N'Spike Chunsoft', CAST(N'2015-12-10T00:00:00.000' AS DateTime), 16, 9.2, N'310K', NULL, N'7 GB', 9.0, 1, CAST(N'2024-02-11T00:00:00.000' AS DateTime)),

(34, N'Leisure Suit Larry - Wet Dreams Don''t Dry', N'Game phiêu lưu hài hước người lớn với nhân vật Larry Laffer cố gắng thích nghi với thời đại hiện đại.', N'Leisure Suit Larry - Wet Dreams Don''t Dry đưa nhân vật huyền thoại Larry Laffer từ thập niên 1980s vào thế giới hiện đại. Sau khi thức dậy bí ẩn sau nhiều thập kỷ, Larry phải học cách thích nghi với thế giới của smartphone, mạng xã hội và hẹn hò trực tuyến. Game là một trò chơi phiêu lưu point-and-click đầy hài hước và châm biếm về văn hóa hiện đại.', N'/img/games/larry_wet_dreams_dont_dry.jpg', N'CrazyBunch', N'Assemble Entertainment', CAST(N'2018-11-07T00:00:00.000' AS DateTime), 3, 7.3, N'200K', NULL, N'6 GB', 7.1, 0, CAST(N'2024-02-12T00:00:00.000' AS DateTime)),

(35, N'Needy Streamer Overload', N'Mô phỏng quản lý một streamer với yếu tố tâm lý sâu sắc và nhiều kết thúc khác nhau.', N'Needy Streamer Overload là một trò chơi mô phỏng quản lý độc đáo, nơi người chơi đóng vai "P-chan" - người quản lý và bạn trai của một streamer mới nổi có vấn đề tâm lý tên là "OMGkawaiiAngel". Nhiệm vụ của người chơi là giúp cô ấy trở nên nổi tiếng trên internet, nhưng đồng thời phải cân bằng giữa sự nổi tiếng và sức khỏe tâm thần ngày càng xấu đi của cô. Game nổi bật với 20+ kết thúc khác nhau, từ hạnh phúc đến vô cùng đen tối.', N'/img/games/needy_streamer.jpg', N'WSS Playground', N'WSS Playground', CAST(N'2022-01-21T00:00:00.000' AS DateTime), 6, 8.5, N'380K', NULL, N'1 GB', 8.3, 1, CAST(N'2024-02-13T00:00:00.000' AS DateTime)),

(36, N'Amelie', N'Visual novel kinh dị tâm lý về một cô gái trẻ khám phá những bí mật đen tối trong gia đình mình.', N'Amelie là một visual novel kinh dị tâm lý, kể về Amelie Blake - một cô gái trở về nhà sau nhiều năm xa cách để tham dự đám tang của mẹ mình. Trong quá trình ở lại ngôi nhà cũ, cô bắt đầu khám phá ra những bí mật đáng sợ về gia đình mình và quá khứ bị chôn vùi. Game nổi bật với không khí u ám, cốt truyện sâu sắc và nhiều lựa chọn ảnh hưởng đến kết cục của câu chuyện.', N'/img/games/amelie.jpg', N'Blueheart Studios', N'Dark Path Games', CAST(N'2021-07-30T00:00:00.000' AS DateTime), 8, 7.9, N'140K', NULL, N'2 GB', 7.6, 0, CAST(N'2024-02-14T00:00:00.000' AS DateTime)),

(37, N'Policenauts', N'Visual novel khoa học viễn tưởng của Hideo Kojima về một cảnh sát không gian điều tra vụ án bí ẩn.', N'Policenauts là một visual novel phiêu lưu khoa học viễn tưởng được phát triển bởi Hideo Kojima - cha đẻ series Metal Gear. Game kể về Jonathan Ingram, một cựu "Policenaut" (cảnh sát không gian) thức dậy sau 25 năm ngủ đông và phải điều tra vụ mất tích của vợ cũ. Cốt truyện phức tạp, đầy bất ngờ và mang phong cách điện ảnh đặc trưng của Kojima đã khiến game trở thành tác phẩm cult classic trong lòng người hâm mộ.', N'/img/games/policenauts.jpg', N'Konami', N'Konami', CAST(N'1996-01-19T00:00:00.000' AS DateTime), 16, 8.8, N'130K', NULL, N'1.5 GB', 8.6, 1, CAST(N'2024-02-15T00:00:00.000' AS DateTime)),

(38, N'Pokemon Black 2', N'Phần tiếp theo của series Pokémon với vùng đất Unova được mở rộng và nhiều tính năng mới.', N'Pokemon Black 2 là phần tiếp theo trực tiếp của Pokemon Black, diễn ra hai năm sau các sự kiện ở game gốc. Người chơi khám phá vùng đất Unova đã thay đổi, với những thành phố mới, gym leader mới và tổ chức phản diện Team Plasma tái xuất với mục tiêu mới. Game bổ sung nhiều tính năng mới như Pokémon World Tournament, PokéStar Studios và Medal system, đồng thời mở rộng Pokédex với nhiều Pokémon từ các thế hệ trước.', N'/img/games/pokemon_black2.jpg', N'Game Freak', N'Nintendo', CAST(N'2012-10-07T00:00:00.000' AS DateTime), 2, 8.7, N'1.5M', NULL, N'512 MB', 8.5, 1, CAST(N'2024-02-16T00:00:00.000' AS DateTime));

SET IDENTITY_INSERT [dbo].[Games] OFF

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
