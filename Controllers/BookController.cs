using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BookApi.Model;

namespace BookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private static readonly string constr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Books;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
       
        [HttpGet]
        public IActionResult Get()
        {
            Response StatusResponse = new Response();
            List<Books> bookList = new List<Books>();
            using(SqlConnection conn = new SqlConnection(constr))
            {
                //string GetBook = "Select * from Book";
                SqlCommand cmd = new SqlCommand("GetBook", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Books book = new Books();
                    book.Id = Convert.ToInt32(reader["id"]);
                    book.Title = reader["Title"].ToString();
                    book.Author = reader["Author"].ToString();
                    book.ISBN = reader["ISBN"].ToString();
                    book.PublishDate = (DateTime)reader["PublishDate"];
                    bookList.Add(book);
                    StatusResponse.Status = Response.StatusCode;
                    StatusResponse.Data = bookList;

                }
                conn.Close();
            }

            return Ok(StatusResponse);
        }

        // GET By Id
        [HttpGet("{id}")]
        public Books Get(int id)
        {
            Books book = new Books();

            using (SqlConnection conn = new SqlConnection(constr))
            {
                string GetByIdBook = "SELECT * FROM Book WHERE Book.id= " + id;
                SqlCommand cmd = new SqlCommand(GetByIdBook, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    book.Id = Convert.ToInt32(reader["id"]);
                    book.Title = reader["Title"].ToString();
                    book.ISBN = reader["ISBN"].ToString();
                    book.PublishDate = (DateTime)reader["PublishDate"];
                    
                }
                conn.Close();
            }
            return book; 
        }

        // POST 
        [HttpPost]
        public void Post([FromBody] Books book)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("InsertBook", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
                cmd.Parameters.AddWithValue("@PublishDate", book.PublishDate);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        // PUT api/<BookController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Books book)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("PutBook", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", book.Id);
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
                cmd.Parameters.AddWithValue("@PublishDate", book.PublishDate);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        // DELETE api/<BookController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Books book = new Books();
            using (SqlConnection conn = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("DeleteBook", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
