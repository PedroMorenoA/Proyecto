using ConsolaScrap.CAPADATOS;
using ConsolaScrap.CAPAPRESENTACION;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Globalization;
using System.Threading;

namespace ConsolaScrap
{
    class Program
    {
        static DataTable Tabla = new DataTable();
        static MetodosExtraccion metodos = new MetodosExtraccion();


        [Obsolete]
        static void Main(string[] args)
        {


            /*   string sql = "select * from productos";
               Tabla = transac.Listar(sql);
               using (DataTableReader reader = new DataTableReader(
                 new DataTable[] { Tabla }))
               {
                   // Print the contents of each of the result sets.
                   do
                   {
                       PrintColumns(reader);
                   } while (reader.NextResult());
               }*/

            #region FASE 1 

            ChromeDriver driver = new ChromeDriver();
            driver.Url = "https://www.amazon.com/";
            var link = driver.FindElement(By.Id("twotabsearchtextbox"));
            link.SendKeys("portatil");

            var buscar = driver.FindElement(By.ClassName("nav-input"));
            buscar.Click();

            INavigation nav = driver.Navigate();

            #endregion

            #region FASE 2


            IList<IWebElement> productos2 = driver.FindElements(By.XPath("//span[@class='a-size-medium a-color-base a-text-normal']"));
            metodos.bucleProductos(productos2, driver, nav);
            Thread.Sleep(2000);
            IList<IWebElement> productos = driver.FindElements(By.XPath("//span[@class='a-size-base-plus a-color-base a-text-normal']"));
            metodos.bucleProductos(productos, driver, nav);

            #endregion
        }

  

    }
}


 






