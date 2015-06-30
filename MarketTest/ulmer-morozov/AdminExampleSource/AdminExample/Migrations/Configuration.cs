using System;
using System.Data.Entity.Migrations;
using System.Linq;
using Core.Models;
using Data;
using Helpers;
using Helpers.Extensions;

namespace AdminExample.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "AdminExample.App_Data.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //товары
            var childBicycle = new Product(1, "Велосипед детский", "Освежите в памяти воспоминания о детстве, купите себе сами новый трехколесный велосипед, ну, или, попросите у вашей мамы.", "images/products/bicycle.png", 14990);
            var fastBicycle = new Product(2, "Велосипед быстрый", "Высушите голову за 5 минут, без использования фена!", "images/products/speedBicycle.jpg", 29990);
            var rollerSkates = new Product(3, "Роликовые коньки", "Почувствуйте себя коровой на льду!", "images/products/skates.jpg", 5990);
            var rocket = new Product(4, "Ракета", "Полет на луну бесценен!", "images/products/rocket.png", 99999);
            var guitar = new Product(6, "Гитара", "Покорительница противоположенного пола и душа компании.", "images/products/guitar.jpg", 990);
            var scooter = new Product(7, "Самокат", "Обгоните конкурентов на повороте!", "images/products/scooter.jpg", 990);
            var treadmill = new Product(8, "Беговая дорожка", "Нет времени объяснять, надо бежать!", "images/products/treadmill.jpg", 19900);
            var bike = new Product(9, "Мотоцикл", "Незаменимая вешь для косплея терминатора.", "images/products/bike.jpeg", 199900);
            var iphonePack = new Product(10, "Айфоны (упаковка)", "Набор айфонов, отличный подарок для многодетной семьи. Лучше купить всем сразу, чем выслушивать от каждого!", "images/products/iphones.png", 9999);
            var freezer = new Product(21, "Холодильник", "Надежное укрытие во время жары!", "images/products/freezer.jpg", 19999);
            var mangal = new Product(14, "Мангал", "Шашлыки уже трепещут в осинах.", "images/products/mangal.jpg", 229);
            var taburet = new Product(15, "Табурет", "Половина инвентаря из набора для путешествий поросёнка Петра уже ждут Вас.", "images/products/taburet.jpg", 729);
            //аксесуары
            var dvd = new Product(13, "DVD диск", "Запишите на него всё что пожелаете.", "images/products/dvd.jpg", 9, ProductType.Accessorise);
            var ivansHelmet = new Product(16, "Шлем Ивана Грозного", "Отличный шлем для пенной вечеринки.", "images/products/ivans_helmet.jpg", 99999, ProductType.Accessorise);
            taburet.AddAccessorise(ivansHelmet);
            iphonePack.AddAccessorise(ivansHelmet);
            dvd.AddAccessorise(ivansHelmet);
            var bikeHelmet = new Product(17, "Мотоциклетный шлем", "Надежная защита для Вашей головы.", "images/products/bike_helmet.jpg", 9899, ProductType.Accessorise);
            bike.AddAccessorise(bikeHelmet);
            rocket.AddAccessorise(bikeHelmet);
            fastBicycle.AddAccessorise(bikeHelmet);
            var childHelmet = new Product(18, "Детский шлем", "Обязательно купите ребенку!", "images/products/childhelmet.jpg", 7890, ProductType.Accessorise);
            childBicycle.AddAccessorise(childHelmet);
            scooter.AddAccessorise(childHelmet);
            rollerSkates.AddAccessorise(childHelmet);
            var luna = new Product(19, "Луна", "Отлично подойдёт каждому современному человеку!", "images/products/luna.png", 777, ProductType.Accessorise);
            mangal.AddAccessorise(luna);
            rocket.AddAccessorise(luna);
            var childGlasses = new Product(20, "Детские очки", "Чтобы солнце не слепило глаза!", "images/products/childGlasses.jpeg", 199, ProductType.Accessorise);
            childBicycle.AddAccessorise(childGlasses);
            rollerSkates.AddAccessorise(childGlasses);
            scooter.AddAccessorise(childGlasses);
            var banana = new Product(11, "Бананы", "Связка бананов для миньонов!", "images/products/banan.jpg", 99, ProductType.Accessorise);
            mangal.AddAccessorise(banana);
            taburet.AddAccessorise(banana);
            freezer.AddAccessorise(banana);
            var milk = new Product(12, "Молоко", "Молоко вдвойне вкусней, если это молоко!", "images/products/milk.jpg", 59, ProductType.Accessorise);
            mangal.AddAccessorise(milk);
            taburet.AddAccessorise(milk);
            freezer.AddAccessorise(milk);
            var iphoneCase = new Product(22, "Чехол для телефона", "Посещение психотерапевта в подарок!", "images/products/iphoneCase.png", 1199, ProductType.Accessorise);
            iphonePack.AddAccessorise(iphoneCase);
            iphonePack.AddAccessorise(dvd);
            var beerBottle = new Product(23, "Пиво", "Для тех кому надоела кока-кола!", "images/products/beer.jpg", 89, ProductType.Accessorise);
            guitar.AddAccessorise(beerBottle);
            var buben = new Product(5, "Бубен проектный", "Незаменимая вещь для каждого программиста, особенно во времена дедлайнов.", "images/products/buben.jpg", 5000, ProductType.Accessorise);
            mangal.AddAccessorise(buben);
            //
            var newAccessorises = new[]
            {
                ivansHelmet,
                bikeHelmet,
                childHelmet,
                luna,
                childGlasses,
                banana,
                milk,
                iphoneCase,
                beerBottle,
                buben
            };
            foreach (var accessorise in newAccessorises)
            {
                context.Products.Attach(accessorise);
                context.Products.AddOrUpdate(accessorise);
            }
            var newProducts = new[]
            {
                childBicycle,
                fastBicycle,
                rollerSkates,
                rocket,
                guitar,
                scooter,
                treadmill,
                bike,
                iphonePack,
                freezer,
                dvd,
                mangal,
                taburet
            };
            foreach (var product in newProducts)
            {
                context.Products.Attach(product);
                context.Products.AddOrUpdate(product);
            }
            //
            var managers = new[]
            {
               Manager.Create(1,"Мошаров", "Василий", "Петрович", "/images/faces/manager_1.jpg"),
               Manager.Create(2,"Уточкина", "Василиса", "Ивановна", "/images/faces/manager_2.jpg"),
               Manager.Create(3,"Козлова", "Юлия", "Валентиновна", "/images/faces/manager_3.jpg"),
               Manager.Create(4,"Гейзенберг", "Вадим", "Иванович", "/images/faces/manager_4.jpg"),
               Manager.Create(5,"Фролова", "Елена", "Петровна", "/images/faces/manager_5.jpg"),
               Manager.Create(6,"Тесла", "Николай", "Николаевич", "/images/faces/manager_6.jpg"),
               Manager.Create(7,"Карпова", "Светлана", "Евгеньевна", "/images/faces/manager_7.jpg"),
               Manager.Create(8,"Кириллов", "Кирилл", "Кириллович", "/images/faces/manager_8.jpg"),
               Manager.Create(9,"Плеханов", "Игорь", "Семёнович", "/images/faces/manager_9.jpg"),
               Manager.Create(10,"Болд", "Александр", "Васильевич", "/images/faces/manager_10.jpg"),
               Manager.Create(11,"Бенедиктов", "Роман", "Олегович", "/images/faces/manager_11.jpg"),
               Manager.Create(12,"Морозова", "Анастасия", "Дмитриевна", "/images/faces/manager_12.jpg"),
               Manager.Create(13,"Варламова", "Евгения", "Валентиновна", "/images/faces/manager_13.jpg"),
               Manager.Create(14,"Учитель", "Мария", "Викторовна", "/images/faces/manager_14.jpg"),
               Manager.Create(15,"Бекмамбетов", "Семён", "Андреевич", "/images/faces/manager_15.jpg"),
            };
            context.Managers.AddOrUpdate(managers);
            //
            var addresses = new[]
            {
                new Address("Россия", "Москва", "ул. Шарикоподшипниковская", 22),
                new Address("Казахстан", "Алма-Ата", "ул. Сатпаева", 123),
                new Address("Россия", "Санкт-Петербург","ул. Рубенштейна",12),
            };
            var contacts = new[]
            {
                "+7 (123) 555-55-55",
                "fake_address@gamil.com",
                "unhuman_1970@mail.net",
                "+7 (123) 456-78-90",
            };
            var notes = new[]
            {
                "Звонить не позже 19-00!",
                "Звонить не раньше 12-00!!",
                "Обращаться только на Вы.",
                "Обязательно предложите мне какую-нибудь платную услугу.",
                "Захватить по дороге еду в макдональдсе (вместе покушаем)"
            };
            //
            var allOrderItems = context.OrderItems.ToList();
            //Заполним заказами
            for (var orderId = 1; orderId <= 100; orderId++)
            {
                var address = addresses.Shuffle().First();
                var manager = managers.Shuffle().First();
                var contact = contacts.Shuffle().First();
                var note = notes.Shuffle().First();
                var random = new Random();
                var clientName = GenerateRandomName();
                var order = new Order(orderId, clientName, DateTime.Now, manager, address, contact, note);
                const int maxCountOfItems = 5;
                var numberOfItems = random.Next(1, maxCountOfItems);
                Console.WriteLine(numberOfItems);
                var products = newProducts.Shuffle().Take(numberOfItems);
                foreach (var product in products)
                {
                    var itemCount = random.Next(1, 5);
                    order.AddOrderItem(product, itemCount);
                }
                context.Orders.AddOrUpdate(order);
                var newOrderItems = order.Items.ToArray();
                allOrderItems = allOrderItems.Except(newOrderItems).ToList();
                context.OrderItems.AddOrUpdate(newOrderItems);
            }
            context.Set<OrderItem>().RemoveRange(allOrderItems);
            //
            context.SaveChanges();
        }

        private static Name GenerateRandomName()
        {
            var random = new Random();
            var isMale = Convert.ToBoolean(random.Next(0, 1));
            //
            if (isMale)
            {
                return new Name(Generate.RandomMaleLastName(), Generate.RandomMaleName(), Generate.RandomMaleMiddleName());
            }
            return new Name(Generate.RandomFemaleLastName(), Generate.RandomFemaleName(), Generate.RandomFemaleMiddleName());
        }
    }
}
