using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;

namespace internet_shop.Models
{
    public class Parameters
    {
        public DateTime time_order { get; set; }
        public String manager_order { get; set; }
        public String address_order { get; set; }
        public String contact_data { get; set; }
        public String comments { get; set; }
        public String[][] tools { get; set; }
    }

    public class Mymodel
    {
        databaseEntities base_data = new databaseEntities();
        String s = "Ваш номер заказа - ";
        int hash = 1;

        public void Post(Parameters q)
        {
            int g = 0;
            while (g <= 0)
            {
                if (hash <= 0) hash = Math.Abs(hash) / 23;
                hash = hash * 3 + 7;
                var h = from a in base_data.t_order
                        where a.order_number == hash
                        select a.order_number;
                if (h.Count() == 0) g = hash;
            }
            base_data.t_order.Add(new t_order { order_number = g, comment = q.comments, contact_data = q.contact_data, date_order = DateTime.Now, manager_order = q.manager_order, adress = q.address_order });
            for (int i = 0; i < q.tools.Length; i++)
            {
                if (q.tools[i].Length == 1)
                {
                    base_data.t_goods.Add(new t_goods { order_number = g, name_product = q.tools[i][0], id = i });
                }
                else 
                {
                    for (int j = 1; j < q.tools[i].Length; j++)
                    {

                        base_data.t_goods.Add(new t_goods { order_number = g, name_product = q.tools[i][0], name_option = q.tools[i][j], id = i });

                    }
                }
            }
            s = "Ваш номер заказа - " + g;
            base_data.SaveChanges() ;
            return;
        }

        public String[] Get_m()
        {
            var g = from a in base_data.t_manager
                    select a.name_manager;
            var z = g.ToArray();
            for (int i = 0; i < z.Count(); i++)
            {
                z[i] = z[i].Trim();
            }
            return z;
        } 

        public String Get_num() 
        {
            return s;
        }

        public Parameters Get_order(int n)
        {
            Parameters par = new Parameters();
            var g = base_data.t_order.Where(e => e.order_number == n).Select(e => e);
            var gg = g.ToArray();
            if (g.Count() == 0)
            {
                var p = new Parameters();
                p.manager_order = "Not order";
                return p;
            }
            par.address_order = g.First().adress.Trim();
            par.comments = g.First().comment.Trim();
            par.contact_data = g.First().contact_data.Trim();
            par.manager_order = g.First().manager_order;
            par.time_order = g.First().date_order;

            var h = from b in base_data.t_goods
                    where b.order_number == n
                    orderby b.id
                    select b;
            String[][] order = new String[h.Count()][];
            var goods = base_data.t_goods.Select(e => e).Where(e => e.order_number ==  n).OrderBy(e => e.id).ToArray();
            /*for (int y = 0; y < goods.Count(); y++) 
            {
                base_data.t_goods.Remove(goods[y]);
            }
            base_data.t_order.Remove(base_data.t_order.Select(e => e).Where(e => e.order_number == n).First());
            base_data.SaveChanges();*/
            int i = -1, j = 0, id = -1, k = 0;
            while (k < goods.Count())
            {
                if (goods[k].id != id)
                {
                    id = goods[k].id;
                    i++;
                    j = 0;
                    order[i] = new String[100];
                    order[i][j++] = goods[k].name_product;
                    order[i][j++] = goods[k].name_option;
                }
                else
                {
                    order[i][j++] = goods[k].name_option;
                }
                k++;
            }
            par.tools = order;
            return par;
        }

        public String[,] Get()
        {
            //Init();
            var g = from a in base_data.t_product
                    select a.product_name;
            var z = g.ToArray();
            for (int i = 0; i < z.Count(); i++)
            {
                z[i] = z[i].Trim();
            }
            String[,] b = new String[z.Count(), 5];
            for (int i = 0; i < z.Count(); i++)
            {
                b[i, 0] = z[i];
                String par = z[i];
                var h = from c in base_data.t_option
                        where c.id_product == base_data.t_product.Where(o => (o.product_name == par)).Select(o => o.Id).FirstOrDefault()
                        select c.option_name;
                var go = h.ToArray();
                for (int j = 1; j <= go.Count(); j++)
                {   
                    b[i, j] = go[j - 1];
                }
            }
                return b;
        }

        public void Init()
        {
            base_data.t_product.Add(new t_product { Id = 1, product_name = "велосипед", product_price = 2700});
            base_data.t_product.Add(new t_product { Id = 2, product_name = "фотоаппарат", product_price = 2000 });
            base_data.t_product.Add(new t_product { Id = 3, product_name = "лодка", product_price = 7000 });
            base_data.t_product.Add(new t_product { Id = 4, product_name = "роликовые коньки", product_price = 3400 });
            base_data.t_product.Add(new t_product { Id = 5, product_name = "самбовка", product_price = 3000 });

            base_data.t_manager.Add(new t_manager { Id = 1, name_manager = "Петров Константин" });
            base_data.t_manager.Add(new t_manager { Id = 2, name_manager = "Капрелян Армен" });
            base_data.t_manager.Add(new t_manager { Id = 3, name_manager = "Бойко Даниил" });
            base_data.t_manager.Add(new t_manager { Id = 4, name_manager = "Шеропятов Игорь" });

            base_data.t_option.Add(new t_option { Id = 1, id_product = 1, option_name = "багажник", option_price = 300 });
            base_data.t_option.Add(new t_option { Id = 2, id_product = 1, option_name = "насос", option_price = 500 });
            base_data.t_option.Add(new t_option { Id = 3, id_product = 1, option_name = "заднее крыло", option_price = 330 });
            base_data.t_option.Add(new t_option { Id = 4, id_product = 1, option_name = "передний фонарь", option_price = 400 });

            base_data.t_option.Add(new t_option { Id = 5, id_product = 2, option_name = "штатив", option_price = 400 });
            base_data.t_option.Add(new t_option { Id = 6, id_product = 2, option_name = "сменный штатив", option_price = 700 });

            base_data.t_option.Add(new t_option { Id = 7, id_product = 3, option_name = "весло", option_price = 300 });
            base_data.t_option.Add(new t_option { Id = 8, id_product = 3, option_name = "снасти", option_price = 4000 });

            base_data.t_option.Add(new t_option { Id = 9, id_product = 4, option_name = "наколенники", option_price = 600 });
            base_data.t_option.Add(new t_option { Id = 10, id_product = 4, option_name = "шлем", option_price = 600 });

            base_data.t_option.Add(new t_option { Id = 11, id_product = 5, option_name = "пояс", option_price = 300 });
            base_data.t_option.Add(new t_option { Id = 12, id_product = 5, option_name = "бандаж", option_price = 300 });
            base_data.t_option.Add(new t_option { Id = 13, id_product = 5, option_name = "шорты", option_price = 500 });

            base_data.SaveChanges();
        }
    }
}