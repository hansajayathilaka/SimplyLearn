using SimplyLearnServer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyLearnServer
{
    public class DbAccess
    {
        static string connectionString = "Data Source=(local);" +
                                    "Initial Catalog=SimplyLearn;" +
                                    "Integrated Security=SSPI";
        public static int SaveTrainer(Trainer trainer)
        {
            int res = 0;
            SqlConnection cn = null;
            try
            {
                var sessionDt = new DataTable();
                sessionDt.Columns.Add("title", typeof(string));
                sessionDt.Columns.Add("description", typeof(string));

                trainer.Sessions.ForEach(delegate (Session session)
                {
                    sessionDt.Rows.Add(session.Title, session.Description);
                }
                );

                var certificateDt = new DataTable();
                certificateDt.Columns.Add("name", typeof(string));

                trainer.ListOfCertifications.ForEach(delegate (string cetificate)
                {
                    certificateDt.Rows.Add(cetificate);
                }
                );

                using (cn = new SqlConnection { ConnectionString = connectionString })
                {

                    using (var cmd = new SqlCommand
                    {
                        Connection = cn,
                        CommandType = CommandType.StoredProcedure
                    })
                    {
                        cmd.CommandText = "dbo.SP_SAVE_TRAINER";

                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@FirstName", SqlDbType = SqlDbType.NVarChar });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@LastName", SqlDbType = SqlDbType.NVarChar });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@Email", SqlDbType = SqlDbType.NVarChar });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@YearsOfExperience", SqlDbType = SqlDbType.Int });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@HasBlog", SqlDbType = SqlDbType.Bit });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@BlogURL", SqlDbType = SqlDbType.NVarChar });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@BrowserName", SqlDbType = SqlDbType.NVarChar });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@BrowserVersion", SqlDbType = SqlDbType.Int });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@certificationsDt", SqlDbType = SqlDbType.Structured });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@Employer", SqlDbType = SqlDbType.NVarChar });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@RegistrationFee", SqlDbType = SqlDbType.Int });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@SessionsDt", SqlDbType = SqlDbType.Structured });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@Identity", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });

                        cmd.Parameters["@FirstName"].Value = trainer.FirstName;
                        cmd.Parameters["@LastName"].Value = trainer.LastName;
                        cmd.Parameters["@Email"].Value = trainer.Email;
                        cmd.Parameters["@YearsOfExperience"].Value = trainer.YearsOfExperience;
                        cmd.Parameters["@HasBlog"].Value = trainer.HasBlog;
                        cmd.Parameters["@BlogURL"].Value = trainer.LastName;
                        cmd.Parameters["@BrowserName"].Value = trainer.Browser.BrowserName;
                        cmd.Parameters["@BrowserVersion"].Value = trainer.Browser.MajorVersion;
                        cmd.Parameters["@certificationsDt"].Value = certificateDt;
                        cmd.Parameters["@Employer"].Value = trainer.Employer;
                        cmd.Parameters["@RegistrationFee"].Value = trainer.RegistrationFee;
                        cmd.Parameters["@SessionsDt"].Value = sessionDt;

                        cn.Open();

                        cmd.ExecuteScalar();

                        int trainerId = Convert.ToInt32(cmd.Parameters["@Identity"].Value);

                        res = trainerId;

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                res = -1;

            }
            finally
            {
                cn.Dispose();
            }

            return res;
        }

        public static bool LoginUser(User user)
        {
            bool res = false;
            var dt = new DataTable();
            SqlConnection cn = null;
            try
            {
                using (cn = new SqlConnection { ConnectionString = connectionString })
                {

                    using (var cmd = new SqlCommand
                    {
                        Connection = cn,
                        CommandType = CommandType.StoredProcedure
                    })
                    {
                        cmd.CommandText = "dbo.SP_LOGIN_USER";

                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@Username", SqlDbType = SqlDbType.NVarChar });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@Password", SqlDbType = SqlDbType.NVarChar });
                        cmd.Parameters["@Username"].Value = user.Username;
                        cmd.Parameters["@Password"].Value = user.Password;

                        cn.Open();

                        dt.Load(cmd.ExecuteReader());
                        if (dt.Rows.Count == 1 && dt.Rows[0]["Username"].ToString() == user.Username)
                        {
                            res = true;
                        }
                        else
                        {
                            res = false;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                res = false;

            }
            finally
            {
                cn.Dispose();
            }

            return res;
        }
    }
}
