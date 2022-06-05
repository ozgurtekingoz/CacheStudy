using CacheStudy.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace CacheStudy.Services
{
    public class DataAccess
    {
        private string m_ConStr;
        public DataAccess()
        {
            m_ConStr = Startup.StaticConfig.GetConnectionString("WebApiDatabase");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ProductDto> GetAllProducts()
        {
            string query = @"
                            select ProductId, ProductName from
                            dbo.Product
                            ";

            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(m_ConStr))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            List<ProductDto> productList = new List<ProductDto>();
            if (table?.Rows.Count > 0)
            {
                productList = (from DataRow dr in table.Rows
                               select new ProductDto()
                               {
                                   ProductId = Convert.ToInt32(dr["ProductId"]),
                                   ProductName = dr["ProductName"].ToString()
                               }).ToList();

            }
            return productList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ProductDto GetProductById(int productId)
        {
            string query = @"
                            select ProductId, ProductName from
                            dbo.Product where ProductId=@productId
                            ";

            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(m_ConStr))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    SqlParameter name = myCommand.Parameters.Add("@productId", SqlDbType.Int, 15);
                    name.Value = productId;

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            ProductDto product = new ProductDto();
            if (table?.Rows.Count > 0)
            {
                product = new ProductDto()
                {
                    ProductId = Convert.ToInt32(table.Rows[0]["ProductId"]),
                    ProductName = table.Rows[0]["ProductName"].ToString()
                };
            }
            else
            {
                product = new ProductDto()
                {
                    ProductId = -1,
                    ProductName = "Product Not Found"
                };
            }
            return product;
        }
    }
}
