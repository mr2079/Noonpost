using Domain.Entites.Article;
using Domain.Entites.Category;
using Domain.Entites.Comment;
using Domain.Entites.User;
using Domain.Security;
using Microsoft.EntityFrameworkCore;

namespace Application.Context;

public class NoonpostDbContext : DbContext
{
    private long _cId = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmssffff"));

    public NoonpostDbContext(DbContextOptions<NoonpostDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Article> Articles { get; set; } = null!;
    public DbSet<ArticleImage> ArticleImages { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User
        modelBuilder.Entity<User>()
            .Property(u => u.Id).HasColumnName("UserId");
        modelBuilder.Entity<User>()
            .Property(u => u.CreateDate).HasColumnName("RegisterDate");

        // Article
        modelBuilder.Entity<Article>()
            .Property(a => a.Id).HasColumnName("ArticleId");

        // Article Image
        modelBuilder.Entity<ArticleImage>()
            .Property(a => a.Id).HasColumnName("ArticleImageId");

        // Comment
        modelBuilder.Entity<Comment>()
            .Property(c => c.Id).HasColumnName("CommentId");

        // Category
        modelBuilder.Entity<Category>()
            .Property(c => c.Id).HasColumnName("CategoryId");

        // Add Admin to User Entity in Initial Migration
        modelBuilder.Entity<User>()
            .HasData(
                new User()
                {
                    Id = new Guid("52328c6f-f7a5-46b0-bf26-e25fa1beff8a"),
                    CreateDate = DateTime.Now,
                    CId = _cId++,
                    FirstName = "مدیر",
                    LastName = "سامانه",
                    Mobile = "Admin",
                    Role = "Admin",
                    Password = PasswordEncoder.EncodePasswordMd5("Admin"),
                },
                new User()
                {
                    Id = new Guid("0f6ddcbe-a250-401c-8c98-82f7ee33eb7f"),
                    CreateDate = DateTime.Now,
                    CId = _cId++,
                    FirstName = "کاربر",
                    LastName = "1",
                    Mobile = "User1",
                    Role = "User",
                    Password = PasswordEncoder.EncodePasswordMd5("User1"),
                },
                new User()
                {
                    Id = new Guid("f08cb981-86bb-4fc4-8004-9fdbc09521dd"),
                    CreateDate = DateTime.Now,
                    CId = _cId++,
                    FirstName = "نویسنده",
                    LastName = "1",
                    Mobile = "Author1",
                    Role = "Author",
                    Password = PasswordEncoder.EncodePasswordMd5("Author1"),
                }
            );

        modelBuilder.Entity<Category>()
            .HasData(
                new Category()
                {
                    Id = new Guid("2c378103-9a86-40de-a41f-8f9f4b6c23ff"),
                    CId = _cId++,
                    CreateDate = DateTime.Now,
                    Title = "دنیای دیجیتال"
                },
                new Category()
                {
                    Id = new Guid("d812c043-d9a6-4d3b-844d-b6b5585e91f2"),
                    CId = _cId++,
                    CreateDate = DateTime.Now,
                    Title = "بازی و سرگرمی"
                }
            );

        modelBuilder.Entity<Article>()
            .HasData(
                new Article()
                {
                    Id = Guid.NewGuid(),
                    CId = _cId++,
                    CreateDate = DateTime.Now,
                    Title = "اینتل بی\u200cسروصدا تراشه\u200cهای بدون هسته E نسل چهاردهمی خود را معرفی کرد",
                    IsAccepted = true,
                    AuthorId = new Guid("f08cb981-86bb-4fc4-8004-9fdbc09521dd"),
                    CategoryId = new Guid("2c378103-9a86-40de-a41f-8f9f4b6c23ff"),
                    ImageName = "intel.webp",
                    Tags = "اینتل-سی پی یو",
                    Text = "<p>اینتل چند CPU از نسل چهاردهم محصولات خود را بدون به کارگیری هسته\u200cهای بهینه E-Core معرفی کرد که Core i9-14901KE با فرکانس ۵.۸ گیگاهرتز پرچم\u200cدار آن\u200cها است.</p><p>درشرایطی\u200cکه اینتل با مشکلات بزرگی در خصوص عدم پایداری عملکرد پردازنده\u200cهای نسل جدید خود مواجه است، خیلی بی سروصدا اقدام به معرفی چند محصول جدید کرده است که از هسته\u200cهای بهینه\u200cی E استفاده نمی\u200cکنند. پردازنده Core i9-14901KE، پرچم\u200cدار سری محصولات بدون هسته E نسل چهاردهم اینتل به شمار می\u200cرود که فقط از ۸ هسته\u200c عملکرد P-Core برخوردار است و سرعت کلاک آن به ۵.۸ گیگاهرتز می\u200cرسد.</p><p>در خصوص مشخصات سایر مدل\u200cهای این رده نیز باید گفت که در مجموع ۱۱ پردازنده نسل چهاردهمی اینتل از سری E وجود دارند که فقط از معماری هسته\u200cهای P-Core برخوردار هستند. این تراشه\u200cها از سری Raptor Lake محسوب می\u200cشوند که برپایه هسته\u200cهای Raptor Cove طراحی شدند و درهیچ یک از مدل\u200cهای آن\u200cها از هسته\u200cهای Gracemont استفاده نشده است. این درحالی است که سری استاندارد این محصولات تا حداکثر ۱۶ هسته\u200cی Gracemont را در ساختار خود دارند. براساس توضیحات اینتل، این تراشه\u200cها بازار Embedded و Commercial محصولات این شرکت را هدف قرار می\u200cدهند و نباید انتظار فروش عادی آن\u200cها را ازطریق مراکز جمع آوری سیستم داشته باشیم. تمامی مدل\u200cهای ارائه شده در این خانواده عبارت\u200cاند از:</p><ul><li>Core i9-14901KE</li><li>Core i9-14901E</li><li>Core i9-14901TE</li><li>Core i7-14701E</li><li>Core i7-14701TE</li><li>Core i5-14501E</li><li>Core i5-14501TE</li><li>Core i5-14401E</li><li>Core i5-14401TE</li></ul><figure class=\"image\"><img src=\"https://cdn.zoomg.ir/2024/7/intel-core-i9-14901ke-p-core-only-desktop-cpu.png\" alt=\"مشخصات پرچمدار تراشه\u200cهای بدون هسته E نسل چهاردهمی اینتل\"></figure><p>همان\u200cطور که اشاره شد، Core i9-14901KE مدل پرچم\u200cدار سری E است که از هشت هسته\u200cی P-Core و ۱۶ رشته پردازشی برخوردار است و سرعت بیس کلاک آن ۳.۸ گیگاهرتز می\u200cرسد و در بالاترین فرکانس بوست تا ۵.۸ گیگاهرتز نیز افزایش پیدا می\u200cکند. این تراشه از ۳۶ مگابایت حافظه کش L3 و ۱۶ مگابایت حافظه کش L2 برخوردار است توان PL1 TDP آن ۱۲۵ وات اعلام شده است. در اینجا هیچ اشاره\u200cای به توان PL2/PL4 این پردازنده نشده است.</p><p>بنابراین این مقدار احتمالا همان توان مصرفی پیشفرض پردازنده\u200cهای سری E است. مدل Core i9-14901KE از قابلیت\u200cهای کامل اورکلاک پشتیبانی می\u200cکند که باعث می\u200cشود احتمالا شاهد عرضه محدود آن در بازار DIY نیز باشیم. پردازنده\u200cهای دیگری سری Core i7 و Core i5 این خانواده نیز با همان تعداد از هسته\u200c\u200eهای P در مدل استاندارد خود همراه هستند. قرار است همه\u200cی این تراشه\u200cها برای سری T و F محصولات نسل چهاردهم اینتل نیز ارائه شوند. فهرست این محصولات را در ادامه مشاهده می\u200cکنید:</p><figure class=\"image\"><img src=\"https://cdn.zoomg.ir/2024/7/intel.jpg\" alt=\"سایر تراشه\u200cهای بدون هسته E نسل چهاردهمی اینتل\"></figure><p>اینتل رویکرد معرفی پردازنده\u200cهایی که فقط از هسته\u200cهای P استفاده می\u200cکنند را با عرضه محصولاتی مانند Bartlett Lake-S که قرار است در سال ۲۰۲۵ با حداکثر ۱۲ هسته P معرفی شوند نیز ادامه خواهد داد.</p>"
                },
                new Article()
                {
                    Id = Guid.NewGuid(),
                    CId = _cId++,
                    CreateDate = DateTime.Now,
                    Title = "ناشر GTA 6 درباره تأثیر بمباران نظرات منفی بر بازی\u200cها می\u200cگوید",
                    IsAccepted = true,
                    AuthorId = new Guid("f08cb981-86bb-4fc4-8004-9fdbc09521dd"),
                    CategoryId = new Guid("d812c043-d9a6-4d3b-844d-b6b5585e91f2"),
                    ImageName = "gta6.webp",
                    Tags = "جی تی ای-تیک تو-استیم",
                    Text = "<p>شرکت تیک تو اینترتینمنت کمپین\u200cهای بمباران نظرات منفی برای آسیب زدن به جایگاه بازی\u200cها را یک ریسک جدی در تجارت بازی\u200cهای ویدیویی می\u200cداند.</p><p>شرکت Take-Two Interactive در ارتباط با سرمایه\u200cگذاران خود به موضوع خاصی در بازار کنونی بازی\u200cهای ویدیویی پرداخته است و دیدگاه شفاف\u200cتری را نسبت به تأثیرات مستقیم و غیرمستقیم آن در دیگر زمینه\u200cها ارائه می\u200cکند. ناشر بازی GTA 6&nbsp;بمباران نظرات منفی نسبت به بازی\u200cها را یک ریسک جدی در این صنعت توصیف می\u200cکند. منظور از «بمباران نظرات منفی»، ایجاد کمپین\u200cهای بزرگی برای ثبت نظرات منفی در نقد و بررسی بازی\u200cها است که معمولا\u064b نه فقط برای رساندن یک پیام اعتراضی به سازندگان بازی\u200cها توسط جامعه مخاطبان، بلکه بیشتر با هدف آسیب رساندن به جایگاه یک بازی صورت می\u200cگیرد.</p><p>درحالی\u200cکه ظهور پدیده\u200cی بمباران نظرات در نقد و بررسی بازی\u200cها تاحد زیادی با فراگیر شدن حداکثری رسانه\u200cهای اجتماعی و تأثیر آن بر زندگی روزمره همزمان می\u200cشود، قدمت این پدیده به زمانی اولین پلتفرم\u200cهای جمع آوری آنلاین نظرات کاربران ایجاد شدند، برمی\u200cگردد. از اوایل تا اواسط دهه ۲۰۱۰ بود که با ایجاد زمینه\u200cهایی برای ثبت نظرات و انتقادات کاربران توسط استیم، پلی استیشن و ایکس باکس، گیمرها پلتفرم بزرگ\u200cتری را برای شنیده شدن صدای خود توسط بازیسازان دریافت کردند.</p><figure class=\"image\"><img src=\"https://cdn.zoomg.ir/2024/7/xbox-exclusives-review-bombed-metacritic.jpg\" alt=\"نمرات منفی در متاکریتیک\"></figure><p>در طول این سال\u200cها، تیک تو اینتراکتیو در معرض بسیاری از این کمپین\u200cهای نظرات کاربران قرار گرفته است. این غول بزرگ صنعت گیمینگ حالا این پدیده را به\u200cعنوان یک ریسک تجاری پذیرفته است تا مستقیما\u064b تأیید کند که این این قبیل اقدامات کارآمد بوده\u200cاند. Game File گزارش می\u200cدهد که پرونده مشهور به 10K شرکت تیک تو، شامل گزارشی از عملکرد سالانه این شرکت می\u200cشود که در آن از اهمیت حفظ امتیاز و رتبه بندی بازی\u200cها در پلتفرم\u200cهای ترد پارتی و تأثیر آن رو موفقیت یک بازی گفته شده است.</p><p>براین\u200cاساس گفته می\u200cشود که این موضوع در زمینه\u200cهای مختلفی همچون قابلیت دیده شدن یک محصول در پلتفرم مربوطه تأثیر گذار است که در نتیجه\u200cی الگوریتم\u200cهای شناسایی محصولات محبوب کاربران در این پلتفرم\u200cها ایجاد شده است. ازاین\u200cرو به\u200cوجود آمدن کمپین\u200cهایی موسوم به «Defamation campaigns» یا «کمپین\u200cهای تخریب و افترا» با هدف آسیب رساندن به جایگاه یک بازی می\u200cتواند تأثیر مخربی روی عملکرد بازی و در نتیجه از دست دادن بازیکنان و درآمدهای آن داشته باشد.</p><p>سند ارزیابی ریسک تیک تو نشان می\u200cدهد که چنین پدیده\u200cهایی می\u200cتوانند برای شرکت هزینه در پی داشته باشند تا بلکه ازطریق افزایش هزینه\u200cهای بازاریابی خود بتواند با اثرات سوء آن مقابله کند. اگرچه این شرکت خاطر نشان کرد که اگر یک محصول پایین\u200cتر از حدانتظار ظاهر شود، بدیهی است که رگبار نظرات منفی از سوی کاربران گاهی اوقات ممکن است تقصیر عملکرد ضعیف خود سازندگان باشد.</p><p>درواقع این شرکت کمپین\u200c\u200eهای اعتراضی را جدا از این مسئله می\u200cداند که به همان اندازه حائز اهمیت هستند؛ به\u200cعنوان مثال سال گذشته نسخه PS4 و نینتندو سوییچ بازی Red Dead Redemption با بمباران نظرات منفی کاربران مواجه شد؛ اما این اتفاق به\u200cدلیل بد بودن این محصول نبود؛ بلکه به\u200cدلیل این بود که کاربران این بازی را نسخه بازسازی شده\u200cی مناسبی نمی\u200cدانستند.</p><figure class=\"image\"><img src=\"https://cdn.zoomg.ir/2024/7/sony-helldivers-2-psn-account-link-reverse-decision-1.jpg\" alt=\"Helldivers 2 و موضوع لینک کردن حساب PSN سونی\"></figure><p>ناشر GTA 6 تنها شرکت بزرگی نیست که تأیید می\u200cکند نظرات کاربران نسبت به بازی\u200cها در چند سال گذشته روی فروش بازی\u200cها تأثیر زیادی داشته است. سونی هم در ماه می امسال و پس از هجوم گسترده کاربران برای ثبت نظرات منفی نسبت به Helldivers 2 به\u200cدلیل نیاز به لینک شدن اکانت PSN کاربران استیم، اظهارات مشابهی را مطرح کرده بود.</p>"
                },
                new Article()
                {
                    Id = Guid.NewGuid(),
                    CId = _cId++,
                    CreateDate = DateTime.Now,
                    Title = "تاریخ عرضه Valorant برای کنسول\u200cها فاش شد",
                    IsAccepted = true,
                    AuthorId = new Guid("f08cb981-86bb-4fc4-8004-9fdbc09521dd"),
                    CategoryId = new Guid("d812c043-d9a6-4d3b-844d-b6b5585e91f2"),
                    ImageName = "valorant.webp",
                    Tags = "ولورنت-توییتر-نسل 9",
                    Text = "<p>یکی از افشاگرهای سرشناس از زمان احتمالی انتشار بازی Valorant برای کنسول\u200cهای نسل ۹ پرده برداشت.</p><p>ماه گذشته رایت گیمز اعلام کرد که بازی Valorant نهایتا به دنیای کنسول\u200cها می\u200cاید. با اینکه تاریخ انتشار نسخه\u200cهای کنسولی این بازی اعلام نشد، نسخه بتای ولورانت دردسترس کاربران <a href=\"https://www.zoomg.ir/game-articles/329828-sony-playstation-5-games-spec-release-date-buy/\">پلی استیشن 5</a> و <a href=\"https://www.zoomg.ir/game-articles/330159-xbox-series-x-games-spec-release-date-buy/\">ایکس باکس سری ایکس</a> |<a href=\"https://www.zoomg.ir/game-articles/330020-xbox-series-s-games-spec-release-date-buy/\"> ایکس باکس سری اس</a> قرار گرفت. اکنون به\u200cنظر می\u200cرسد که زمان زیادی تا عرضه بازی برای کنسول\u200cها باقی نمانده است.</p><p>همان\u200cطور که eXtas1stv، یکی از افشاگرهای خوش\u200cسابقه در شبکه اجتماعی ایکس (توییتر سابق) ادعا کرد، بتای Valorant به\u200cزودی از دسترس خارج می\u200cشود و نسخه کامل بازی در تاریخ ۳ مرداد ۱۴۰۳ (۲۴ جولای ۲۰۲۴) برای کنسول\u200cها عرضه خواهد شد. شرکت Riot Games تنها به انتشار نسخه کنسولی در زمانی نامعلوم از سال ۲۰۲۴ اشاره کرده است.</p><p>بازی Valorant درحال\u200cحاضر در پلتفرم کامپیوتر قابل تجربه است. شرکت رایت گیمز تأیید کرده است که نسخه پلی استیشن 5 از هدف\u200cگیری ژیروسکوپی پشتیبانی نمی\u200cکند؛ زیرا این ویژگی در کنترلر ایکس باکس وجود ندارد.</p><p>افزون\u200cبراین eXtas1stv زمان راه\u200cیابی Call of Duty: Modern Warfare 3 به گیم پس را نیز فاش کرد. ظاهرا کاربران این سرویس می\u200cتوانند در تاریخ ۳ مرداد ۱۴۰۳ (۲۴ جولای ۲۰۲۴) به\u200cسراغ تجربه مدرن وارفر ۳ بروند.</p>"
                }
            );

        base.OnModelCreating(modelBuilder);
    }
}
