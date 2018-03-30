using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nastart.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;

namespace nastart.Controllers
{
    public class IMGbinarydataController : Controller
    {
        public string strBase64;
        BazaTestowaEntities1 db = new BazaTestowaEntities1();
        Form forma = new Form();
        Ans anwers = new Ans();
        Backend context = new Backend();
        PrzewodnikPoITEntities dbb = new PrzewodnikPoITEntities();
        public ActionResult Index()
        {
            //selectionanswers();
            List<Backend> list = dbb.Backend.ToList();
            ViewBag.Tech = new SelectList(list, "id", "nazwa");
            List<ImageIcon> images = GetImages();
            return View(images);
        }
        public ActionResult Choos()
        {
            return View();
        }
        public ActionResult CheckTechno(Form LanguageId)
        {
            AnswerSubmit(LanguageId);
            return View(LanguageId);
        }
        private void AnswerSubmit(Form LanguageId)
        {

  
            string cs = ConfigurationManager.ConnectionStrings["Przewodnik"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                //  SqlCommand cmd = new SqlCommand("spGetById", con); // jeśli mamy proc w bazie danych
                string querysql = "Select nazwa, text, Image from Backend where id=" + LanguageId.id + "";
                con.Open();
                SqlCommand cmd = new SqlCommand(querysql, con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //SqlParameter paramId = new SqlParameter()
                //{
                //    ParameterName = "@Id",
                //    Value = LanguageId.id
                //};
                SqlDataReader reader1;
                reader1 = cmd.ExecuteReader();
                while (reader1.Read())
                {
                    LanguageId.nazwa = reader1.GetString(0);
                    LanguageId.opis = reader1.GetString(1);
                    byte[] img = (byte[])reader1[2];
                    LanguageId.obraz = "data:image/png;base64," + Convert.ToBase64String(img);

                }
            }
         
        }
        public PartialViewResult Front()
        {
            List<Backend> list = dbb.Backend.ToList();
            ViewBag.Tech = new SelectList(list, "id", "nazwa");
            return PartialView("_Technology");
        }
        public PartialViewResult Back()
        {
            return PartialView("_Technology");
        }
        private List<ImageIcon> GetImages()
        {
            string query = "Select TOP 9 Image from Backend";
            List<ImageIcon> images = new List<ImageIcon>();
            string constr = ConfigurationManager.ConnectionStrings["Przewodnik"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            byte[] img = (byte[])sdr["Image"];
                            images.Add(new ImageIcon
                            {
                                pic = "data:image/png;base64," + Convert.ToBase64String(img),
                            });

                        }
                    }
                    con.Close();
                }

                return images;
            }
        }
    }
}