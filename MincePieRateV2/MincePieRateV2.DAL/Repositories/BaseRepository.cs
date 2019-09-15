﻿using Microsoft.EntityFrameworkCore;
using MincePieRateV2.DAL.Data;
using MincePieRateV2.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MincePieRateV2.DAL.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T: class, new()
    {
        public ApplicationDbContext Context;
        public DbSet<T> Table { get; }

        public BaseRepository(ApplicationDbContext context)
        {
            Context = context;
            Table = context.Set<T>();
        }

        public void Add(T entity)
        {
            Table.Add(entity);
            Context.SaveChanges();
        }

        public T GetEntity(Func<T, bool> predicate)
        {
            return Table.Where(predicate).FirstOrDefault();
        }

        public IEnumerable<T> GetEntities(Func<T, bool> predicate = null)
        {
            if (predicate != null)
            {
                return Table.Where(predicate);
            }
            else
            {
                return Table;
            }
        }

        public void Update(T entity)
        {
            Table.Update(entity);
            Context.SaveChanges();
        }

        public void Delete(T entity)
        {
            Table.Remove(entity);
            Context.SaveChanges();
        }
    }
}
