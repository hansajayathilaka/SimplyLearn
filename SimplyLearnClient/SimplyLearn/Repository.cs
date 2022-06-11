using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimplyLearn
{
    public class Repository: IRepository
    {
        private readonly SimpleTcpClient client;
        public Repository(SimpleTcpClient client)
        {
            this.client = client;
        }

        public int SaveTrainer(Trainer trainer)
        {
            var jsonString = JsonSerializer.Serialize(trainer);
            int res = 0;
            try
            {
                client.Send($"trainer;{jsonString}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                res = -1;

            }
            finally
            {

            }

            return res;
        }

        public bool LoginUser(User user)
        {
            bool res = false;
            try
            {
                var jsonString = JsonSerializer.Serialize(user);
                client.Send($"login;{jsonString}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                res = false;

            }
            finally
            {
            }

            return res;
        }
    }
}
