using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ConsolaScrap.CAPADATOS;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ConsolaScrap.CAPAPRESENTACION
{
    class MetodosExtraccion
    {
        static Transacciones transac = new Transacciones();
        static bool correct = false;
        public void bucleProductos(IList<IWebElement> productos, ChromeDriver driver, INavigation nav)
        {
            int cont = productos.Count;


            if (cont > 0)
            {
                correct = true;
            }

            if (correct)
            {
                String[] allText = new string[cont];

                int i = 0;
                foreach (IWebElement producto in productos)
                {
                    allText[i] = producto.Text;
                    Console.WriteLine(allText[i]);

                    i++;
                }
                //   Console.WriteLine(i);

                IWebElement linkProducto;

                foreach (string nombre in allText)
                {
                    try
                    {
                        Thread.Sleep(4000);
                        linkProducto = driver.FindElement(By.XPath(xpathToFind: "//a[span='" + nombre + "']"));
                        Thread.Sleep(1000);
                        linkProducto.Click();
                        Console.WriteLine(nombre);

                        try
                        {
                            for (int a = 0; a < 2; a++)
                            {
                                try
                                {
                                    IWebElement masReseñas = driver.FindElement(By.XPath("//span[span='Cargar más reseñas internacionales']")); ;
                                    masReseñas.Click();
                                }
                                catch (Exception e)
                                {
                                }

                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("no hay más botones de reseñas" + e);
                        }

                        string SnumReseñas;
                        int numReseñas = 0;
                        string Sprecio = null;
                        string cadenaPro;

                        Thread.Sleep(2000);
                        try
                        {
                            Sprecio = driver.FindElement(By.Id("priceblock_ourprice")).Text;
                            SnumReseñas = driver.FindElement(By.Id("acrCustomerReviewText")).Text;
                            numReseñas = (int)numReseñasEntero(SnumReseñas);
                        }
                        catch (NoSuchElementException e)
                        {
                            Sprecio = driver.FindElement(By.Id("priceblock_saleprice")).Text;
                            SnumReseñas = driver.FindElement(By.Id("acrCustomerReviewText")).Text;
                            numReseñas = (int)numReseñasEntero(SnumReseñas);
                        }

                        //Excepción causada al no poder insertar producto
                        try
                        {

                            cadenaPro = "insert into productos (nombre, precio,  numReseñas) values ('" + nombre + "','" + Sprecio + "'," + numReseñas + ")";
                            transac.InsertarProducto(cadenaPro);

                            //  Console.WriteLine(cadenaPro);

                            //--------------------------------------------------------------USUARIOS Y RESEÑAS--------------------------------------------------------------

                            //Excepción causada por no encontrar perfil de clientes
                            try
                            {
                                Thread.Sleep(3000);

                                IList<IWebElement> perfiles = driver.FindElements(By.XPath("//span[@class='a-profile-name']"));
                                int contaPerfiles = perfiles.Count;
                                String[] nombres = new string[contaPerfiles];
                                nombres = limpiarArray(perfiles, nombres);

                                IList<IWebElement> Ireseñas = driver.FindElements(By.XPath("//div[@class='a-expander-content reviewText review-text-content a-expander-partial-collapse-content']"));
                                int contaReseñas = Ireseñas.Count;
                                String[] reseñas = new string[contaReseñas];
                                reseñas = limpiarArray(Ireseñas, reseñas);

                                string cadenaUsu;
                                string rparam;

                                for (int y = 0; y < nombres.Length; y++)
                                {

                                    //Excepción causada por no encontrar perfil de clientes
                                    try
                                    {
                                        Console.WriteLine(nombres[y]);

                                        if (nombre[y].Equals(""))
                                        {
                                            Console.WriteLine("nombre Vacio");
                                        }
                                        else
                                        {
                                            try
                                            {
                                                cadenaUsu = "insert into usuarios (nombre) values ('" + nombres[y] + "')";
                                                transac.InsertarUsuario(nombres[y]);
                                                Console.WriteLine(cadenaUsu);
                                                Console.WriteLine("-----------------USUARIO INTRODUCIDO-----------------");

                                                Thread.Sleep(500);

                                                string cadenaReseñas = "select idProducto from productos where nombre='" + nombre + "'";
                                                int idProducto = transac.Seleccionar(cadenaReseñas);
                                                string cadenaUsuarios = "select idUsuario from usuarios where nombre='" + nombres[y] + "'";
                                                int idUsuario = transac.Seleccionar(cadenaUsuarios);
                                                Console.WriteLine(reseñas[y]);

                                                rparam = reseñas[y];

                                                transac.InsertarReseña(idProducto, idUsuario, rparam);
                                                Console.WriteLine("-----------------RESEÑA INTRODUCIDA-----------------");
                                            }
                                            catch (Exception e)
                                            {
                                                Console.WriteLine(e);
                                            }

                                            Thread.Sleep(1000);
                                        }

                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("----------------------------- Error: " + e);
                                    }

                                }
                            }
                            catch (System.Data.SqlClient.SqlException e)
                            {
                                Console.WriteLine("SQL EXCEPTION--------------------------------" + e + "--------------------------------");
                            }



                        }
                        catch (NoSuchElementException)
                        {

                        }

                        nav.Back();
                        Thread.Sleep(1000);
                    }

                    //Excepción causada por no encontrar enlace a producto
                    catch (NoSuchElementException e)
                    {
                        Console.WriteLine("PRODUCT EXCEPTION" + e);
                    }

                }
                correct = false;

            }
            else
            {
                correct = true;
            }
        }
        public static double numReseñasEntero(string pre)
        {

            double numero;
            string comp = string.Empty;

            int i = 0;
            while (i < 3)
            {

                if (Char.IsDigit(pre[i]))
                {

                    comp += pre[i];
                }
                i++;
            }

            numero = Convert.ToDouble(comp);
            return numero;
        }



        static String[] limpiarArray(IList<IWebElement> lista, String[] array)
        {

            int h = 0;
            foreach (IWebElement element in lista)
            {
                if (element.Text.Equals("") || element.Text == null)
                {

                }
                else
                {
                    array[h] = element.Text;
                    h++;
                }

            }
            Array.Resize(ref array, h);
            Console.WriteLine(array.Length);
            return array;

        }
        /*    static void reseñasUsuarios(ChromeDriver driver, String[] reseñas)
            {



                int x = 0;
                foreach (IWebElement element in perfiles)
                {
                    if (element.Text.Equals("") || element.Text == null)
                    {

                    }
                    else
                    {
                        reseñas[x] = element.Text;
                        x++;
                    }

                }
                Array.Resize(ref reseñas, x);
                Console.WriteLine(reseñas.Length);
            }
            */


     /*   private static void PrintColumns(DataTableReader reader)
        {
            // Loop through all the rows in the DataTableReader
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write(reader[i] + " ");
                }
                Console.WriteLine();
            }
        }
        */
    }

}
