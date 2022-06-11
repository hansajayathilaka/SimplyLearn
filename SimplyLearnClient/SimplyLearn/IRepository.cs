using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimplyLearn
{
    public interface IRepository
    {
        int SaveTrainer(Trainer trainer);
        bool LoginUser(User user);
    }
}
