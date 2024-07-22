using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleImages",
                columns: table => new
                {
                    ArticleImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArticleImageGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CId = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleImages", x => x.ArticleImageId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CId = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CId = table.Column<long>(type: "bigint", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImagesGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImageName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    View = table.Column<int>(type: "int", nullable: false),
                    CId = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ArticleId);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Articles_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    CId = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId");
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CId", "CreateDate", "Title", "UpdateDate" },
                values: new object[,]
                {
                    { new Guid("2c378103-9a86-40de-a41f-8f9f4b6c23ff"), 202407222302526825L, new DateTime(2024, 7, 22, 23, 2, 53, 119, DateTimeKind.Local).AddTicks(5783), "دنیای دیجیتال", null },
                    { new Guid("d812c043-d9a6-4d3b-844d-b6b5585e91f2"), 202407222302526826L, new DateTime(2024, 7, 22, 23, 2, 53, 119, DateTimeKind.Local).AddTicks(5786), "بازی و سرگرمی", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CId", "RegisterDate", "Description", "Email", "FirstName", "ImageName", "LastName", "Mobile", "Password", "Role", "UpdateDate" },
                values: new object[,]
                {
                    { new Guid("0f6ddcbe-a250-401c-8c98-82f7ee33eb7f"), 202407222302526823L, new DateTime(2024, 7, 22, 23, 2, 53, 119, DateTimeKind.Local).AddTicks(5678), "", null, "کاربر", "Default.jpg", "1", "User1", "6B-90-8B-78-5F-DB-A0-5A-64-46-34-7D-AE-08-D8-C5", "User", null },
                    { new Guid("52328c6f-f7a5-46b0-bf26-e25fa1beff8a"), 202407222302526822L, new DateTime(2024, 7, 22, 23, 2, 53, 119, DateTimeKind.Local).AddTicks(5537), "", null, "مدیر", "Default.jpg", "سامانه", "Admin", "E3-AF-ED-00-47-B0-80-59-D0-FA-DA-10-F4-00-C1-E5", "Admin", null },
                    { new Guid("f08cb981-86bb-4fc4-8004-9fdbc09521dd"), 202407222302526824L, new DateTime(2024, 7, 22, 23, 2, 53, 119, DateTimeKind.Local).AddTicks(5704), "", null, "نویسنده", "Default.jpg", "1", "Author1", "8D-C0-4F-F6-21-E2-96-10-2C-5B-FB-06-4E-6B-BA-D8", "Author", null }
                });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "ArticleId", "AuthorId", "CId", "CategoryId", "CreateDate", "ImageName", "ImagesGuid", "IsAccepted", "Tags", "Text", "Title", "UpdateDate", "View" },
                values: new object[] { new Guid("cd723c27-197a-4f7c-8649-8bd67802e94b"), new Guid("f08cb981-86bb-4fc4-8004-9fdbc09521dd"), 202407222302526828L, new Guid("d812c043-d9a6-4d3b-844d-b6b5585e91f2"), new DateTime(2024, 7, 22, 23, 2, 53, 119, DateTimeKind.Local).AddTicks(5817), "gta6.webp", null, true, "جی تی ای-تیک تو-استیم", "<p>شرکت تیک تو اینترتینمنت کمپین‌های بمباران نظرات منفی برای آسیب زدن به جایگاه بازی‌ها را یک ریسک جدی در تجارت بازی‌های ویدیویی می‌داند.</p><p>شرکت Take-Two Interactive در ارتباط با سرمایه‌گذاران خود به موضوع خاصی در بازار کنونی بازی‌های ویدیویی پرداخته است و دیدگاه شفاف‌تری را نسبت به تأثیرات مستقیم و غیرمستقیم آن در دیگر زمینه‌ها ارائه می‌کند. ناشر بازی GTA 6&nbsp;بمباران نظرات منفی نسبت به بازی‌ها را یک ریسک جدی در این صنعت توصیف می‌کند. منظور از «بمباران نظرات منفی»، ایجاد کمپین‌های بزرگی برای ثبت نظرات منفی در نقد و بررسی بازی‌ها است که معمولاً نه فقط برای رساندن یک پیام اعتراضی به سازندگان بازی‌ها توسط جامعه مخاطبان، بلکه بیشتر با هدف آسیب رساندن به جایگاه یک بازی صورت می‌گیرد.</p><p>درحالی‌که ظهور پدیده‌ی بمباران نظرات در نقد و بررسی بازی‌ها تاحد زیادی با فراگیر شدن حداکثری رسانه‌های اجتماعی و تأثیر آن بر زندگی روزمره همزمان می‌شود، قدمت این پدیده به زمانی اولین پلتفرم‌های جمع آوری آنلاین نظرات کاربران ایجاد شدند، برمی‌گردد. از اوایل تا اواسط دهه ۲۰۱۰ بود که با ایجاد زمینه‌هایی برای ثبت نظرات و انتقادات کاربران توسط استیم، پلی استیشن و ایکس باکس، گیمرها پلتفرم بزرگ‌تری را برای شنیده شدن صدای خود توسط بازیسازان دریافت کردند.</p><figure class=\"image\"><img src=\"https://cdn.zoomg.ir/2024/7/xbox-exclusives-review-bombed-metacritic.jpg\" alt=\"نمرات منفی در متاکریتیک\"></figure><p>در طول این سال‌ها، تیک تو اینتراکتیو در معرض بسیاری از این کمپین‌های نظرات کاربران قرار گرفته است. این غول بزرگ صنعت گیمینگ حالا این پدیده را به‌عنوان یک ریسک تجاری پذیرفته است تا مستقیماً تأیید کند که این این قبیل اقدامات کارآمد بوده‌اند. Game File گزارش می‌دهد که پرونده مشهور به 10K شرکت تیک تو، شامل گزارشی از عملکرد سالانه این شرکت می‌شود که در آن از اهمیت حفظ امتیاز و رتبه بندی بازی‌ها در پلتفرم‌های ترد پارتی و تأثیر آن رو موفقیت یک بازی گفته شده است.</p><p>براین‌اساس گفته می‌شود که این موضوع در زمینه‌های مختلفی همچون قابلیت دیده شدن یک محصول در پلتفرم مربوطه تأثیر گذار است که در نتیجه‌ی الگوریتم‌های شناسایی محصولات محبوب کاربران در این پلتفرم‌ها ایجاد شده است. ازاین‌رو به‌وجود آمدن کمپین‌هایی موسوم به «Defamation campaigns» یا «کمپین‌های تخریب و افترا» با هدف آسیب رساندن به جایگاه یک بازی می‌تواند تأثیر مخربی روی عملکرد بازی و در نتیجه از دست دادن بازیکنان و درآمدهای آن داشته باشد.</p><p>سند ارزیابی ریسک تیک تو نشان می‌دهد که چنین پدیده‌هایی می‌توانند برای شرکت هزینه در پی داشته باشند تا بلکه ازطریق افزایش هزینه‌های بازاریابی خود بتواند با اثرات سوء آن مقابله کند. اگرچه این شرکت خاطر نشان کرد که اگر یک محصول پایین‌تر از حدانتظار ظاهر شود، بدیهی است که رگبار نظرات منفی از سوی کاربران گاهی اوقات ممکن است تقصیر عملکرد ضعیف خود سازندگان باشد.</p><p>درواقع این شرکت کمپین‌‎های اعتراضی را جدا از این مسئله می‌داند که به همان اندازه حائز اهمیت هستند؛ به‌عنوان مثال سال گذشته نسخه PS4 و نینتندو سوییچ بازی Red Dead Redemption با بمباران نظرات منفی کاربران مواجه شد؛ اما این اتفاق به‌دلیل بد بودن این محصول نبود؛ بلکه به‌دلیل این بود که کاربران این بازی را نسخه بازسازی شده‌ی مناسبی نمی‌دانستند.</p><figure class=\"image\"><img src=\"https://cdn.zoomg.ir/2024/7/sony-helldivers-2-psn-account-link-reverse-decision-1.jpg\" alt=\"Helldivers 2 و موضوع لینک کردن حساب PSN سونی\"></figure><p>ناشر GTA 6 تنها شرکت بزرگی نیست که تأیید می‌کند نظرات کاربران نسبت به بازی‌ها در چند سال گذشته روی فروش بازی‌ها تأثیر زیادی داشته است. سونی هم در ماه می امسال و پس از هجوم گسترده کاربران برای ثبت نظرات منفی نسبت به Helldivers 2 به‌دلیل نیاز به لینک شدن اکانت PSN کاربران استیم، اظهارات مشابهی را مطرح کرده بود.</p>", "ناشر GTA 6 درباره تأثیر بمباران نظرات منفی بر بازی‌ها می‌گوید", null, 0 });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "ArticleId", "AuthorId", "CId", "CategoryId", "CreateDate", "ImageName", "ImagesGuid", "IsAccepted", "Tags", "Text", "Title", "UpdateDate", "View" },
                values: new object[] { new Guid("d832dbd1-8703-4513-97a1-a295a15cd1db"), new Guid("f08cb981-86bb-4fc4-8004-9fdbc09521dd"), 202407222302526829L, new Guid("d812c043-d9a6-4d3b-844d-b6b5585e91f2"), new DateTime(2024, 7, 22, 23, 2, 53, 119, DateTimeKind.Local).AddTicks(5820), "valorant.webp", null, true, "ولورنت-توییتر-نسل 9", "<p>یکی از افشاگرهای سرشناس از زمان احتمالی انتشار بازی Valorant برای کنسول‌های نسل ۹ پرده برداشت.</p><p>ماه گذشته رایت گیمز اعلام کرد که بازی Valorant نهایتا به دنیای کنسول‌ها می‌اید. با اینکه تاریخ انتشار نسخه‌های کنسولی این بازی اعلام نشد، نسخه بتای ولورانت دردسترس کاربران <a href=\"https://www.zoomg.ir/game-articles/329828-sony-playstation-5-games-spec-release-date-buy/\">پلی استیشن 5</a> و <a href=\"https://www.zoomg.ir/game-articles/330159-xbox-series-x-games-spec-release-date-buy/\">ایکس باکس سری ایکس</a> |<a href=\"https://www.zoomg.ir/game-articles/330020-xbox-series-s-games-spec-release-date-buy/\"> ایکس باکس سری اس</a> قرار گرفت. اکنون به‌نظر می‌رسد که زمان زیادی تا عرضه بازی برای کنسول‌ها باقی نمانده است.</p><p>همان‌طور که eXtas1stv، یکی از افشاگرهای خوش‌سابقه در شبکه اجتماعی ایکس (توییتر سابق) ادعا کرد، بتای Valorant به‌زودی از دسترس خارج می‌شود و نسخه کامل بازی در تاریخ ۳ مرداد ۱۴۰۳ (۲۴ جولای ۲۰۲۴) برای کنسول‌ها عرضه خواهد شد. شرکت Riot Games تنها به انتشار نسخه کنسولی در زمانی نامعلوم از سال ۲۰۲۴ اشاره کرده است.</p><p>بازی Valorant درحال‌حاضر در پلتفرم کامپیوتر قابل تجربه است. شرکت رایت گیمز تأیید کرده است که نسخه پلی استیشن 5 از هدف‌گیری ژیروسکوپی پشتیبانی نمی‌کند؛ زیرا این ویژگی در کنترلر ایکس باکس وجود ندارد.</p><p>افزون‌براین eXtas1stv زمان راه‌یابی Call of Duty: Modern Warfare 3 به گیم پس را نیز فاش کرد. ظاهرا کاربران این سرویس می‌توانند در تاریخ ۳ مرداد ۱۴۰۳ (۲۴ جولای ۲۰۲۴) به‌سراغ تجربه مدرن وارفر ۳ بروند.</p>", "تاریخ عرضه Valorant برای کنسول‌ها فاش شد", null, 0 });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "ArticleId", "AuthorId", "CId", "CategoryId", "CreateDate", "ImageName", "ImagesGuid", "IsAccepted", "Tags", "Text", "Title", "UpdateDate", "View" },
                values: new object[] { new Guid("efd3a476-fa85-4290-8f28-3af7722afd48"), new Guid("f08cb981-86bb-4fc4-8004-9fdbc09521dd"), 202407222302526827L, new Guid("2c378103-9a86-40de-a41f-8f9f4b6c23ff"), new DateTime(2024, 7, 22, 23, 2, 53, 119, DateTimeKind.Local).AddTicks(5812), "intel.webp", null, true, "اینتل-سی پی یو", "<p>اینتل چند CPU از نسل چهاردهم محصولات خود را بدون به کارگیری هسته‌های بهینه E-Core معرفی کرد که Core i9-14901KE با فرکانس ۵.۸ گیگاهرتز پرچم‌دار آن‌ها است.</p><p>درشرایطی‌که اینتل با مشکلات بزرگی در خصوص عدم پایداری عملکرد پردازنده‌های نسل جدید خود مواجه است، خیلی بی سروصدا اقدام به معرفی چند محصول جدید کرده است که از هسته‌های بهینه‌ی E استفاده نمی‌کنند. پردازنده Core i9-14901KE، پرچم‌دار سری محصولات بدون هسته E نسل چهاردهم اینتل به شمار می‌رود که فقط از ۸ هسته‌ عملکرد P-Core برخوردار است و سرعت کلاک آن به ۵.۸ گیگاهرتز می‌رسد.</p><p>در خصوص مشخصات سایر مدل‌های این رده نیز باید گفت که در مجموع ۱۱ پردازنده نسل چهاردهمی اینتل از سری E وجود دارند که فقط از معماری هسته‌های P-Core برخوردار هستند. این تراشه‌ها از سری Raptor Lake محسوب می‌شوند که برپایه هسته‌های Raptor Cove طراحی شدند و درهیچ یک از مدل‌های آن‌ها از هسته‌های Gracemont استفاده نشده است. این درحالی است که سری استاندارد این محصولات تا حداکثر ۱۶ هسته‌ی Gracemont را در ساختار خود دارند. براساس توضیحات اینتل، این تراشه‌ها بازار Embedded و Commercial محصولات این شرکت را هدف قرار می‌دهند و نباید انتظار فروش عادی آن‌ها را ازطریق مراکز جمع آوری سیستم داشته باشیم. تمامی مدل‌های ارائه شده در این خانواده عبارت‌اند از:</p><ul><li>Core i9-14901KE</li><li>Core i9-14901E</li><li>Core i9-14901TE</li><li>Core i7-14701E</li><li>Core i7-14701TE</li><li>Core i5-14501E</li><li>Core i5-14501TE</li><li>Core i5-14401E</li><li>Core i5-14401TE</li></ul><figure class=\"image\"><img src=\"https://cdn.zoomg.ir/2024/7/intel-core-i9-14901ke-p-core-only-desktop-cpu.png\" alt=\"مشخصات پرچمدار تراشه‌های بدون هسته E نسل چهاردهمی اینتل\"></figure><p>همان‌طور که اشاره شد، Core i9-14901KE مدل پرچم‌دار سری E است که از هشت هسته‌ی P-Core و ۱۶ رشته پردازشی برخوردار است و سرعت بیس کلاک آن ۳.۸ گیگاهرتز می‌رسد و در بالاترین فرکانس بوست تا ۵.۸ گیگاهرتز نیز افزایش پیدا می‌کند. این تراشه از ۳۶ مگابایت حافظه کش L3 و ۱۶ مگابایت حافظه کش L2 برخوردار است توان PL1 TDP آن ۱۲۵ وات اعلام شده است. در اینجا هیچ اشاره‌ای به توان PL2/PL4 این پردازنده نشده است.</p><p>بنابراین این مقدار احتمالا همان توان مصرفی پیشفرض پردازنده‌های سری E است. مدل Core i9-14901KE از قابلیت‌های کامل اورکلاک پشتیبانی می‌کند که باعث می‌شود احتمالا شاهد عرضه محدود آن در بازار DIY نیز باشیم. پردازنده‌های دیگری سری Core i7 و Core i5 این خانواده نیز با همان تعداد از هسته‌‎های P در مدل استاندارد خود همراه هستند. قرار است همه‌ی این تراشه‌ها برای سری T و F محصولات نسل چهاردهم اینتل نیز ارائه شوند. فهرست این محصولات را در ادامه مشاهده می‌کنید:</p><figure class=\"image\"><img src=\"https://cdn.zoomg.ir/2024/7/intel.jpg\" alt=\"سایر تراشه‌های بدون هسته E نسل چهاردهمی اینتل\"></figure><p>اینتل رویکرد معرفی پردازنده‌هایی که فقط از هسته‌های P استفاده می‌کنند را با عرضه محصولاتی مانند Bartlett Lake-S که قرار است در سال ۲۰۲۵ با حداکثر ۱۲ هسته P معرفی شوند نیز ادامه خواهد داد.</p>", "اینتل بی‌سروصدا تراشه‌های بدون هسته E نسل چهاردهمی خود را معرفی کرد", null, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_AuthorId",
                table: "Articles",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryId",
                table: "Articles",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleId",
                table: "Comments",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentId",
                table: "Comments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleImages");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
