using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Payroll_Manager.Persistence;

namespace Payroll_Manager.Services
{
  /// <summary>
  /// UnitOfWork pattern for auction database
  /// </summary>
  public class UnitOfWork : IDisposable
  {
    private readonly ApplicationDbContext _db;

    public UnitOfWork(ApplicationDbContext db)
    {
      _db = db;
    }


    public IDbContextTransaction BeginTransaction()
    {
      return _db.Database.BeginTransaction();
    }

    public async Task SaveAsync()
    {
      await _db.SaveChangesAsync();
    }

    private bool _disposed = false;

    public virtual void Dispose(bool disposing)
    {
      if (_disposed)
        return;

      if (disposing)
      {
        _db.Dispose();
      }

      _disposed = true;
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
  }
}
