﻿using DAL.Datos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class EFRepository : IRepository
    {
        //Llama a la clase de contexto
        ApplicationDbContext _context;

        //Constructor
        public EFRepository(ApplicationDbContext context)
        {
            this._context = context;
        }
        //Dispose
        private bool disposedValue;
        public async Task<TEntity> CreateAsync<TEntity>(TEntity toCreate) where TEntity : class
        {
            TEntity Result = default(TEntity);
            try
            {
                await _context.Set<TEntity>().AddAsync(toCreate);
                await _context.SaveChangesAsync();
                Result = toCreate;
            }
            catch (DbException)
            {
                throw;
            }
            return Result;
        }

        public async Task<bool> DeleteAsync<TEntity>(TEntity toDelete) where TEntity : class
        {
            bool Result = false;
            try
            {
                _context.Entry<TEntity>(toDelete).State = EntityState.Deleted;
                Result = await _context.SaveChangesAsync() > 0;
            }
            catch (DbException)
            {
                throw;
            }
            return Result;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> FilterAsync<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> RetreiveAsync<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync<TEntity>(TEntity toUpdate) where TEntity : class
        {
            throw new NotImplementedException();
        }
    }
}