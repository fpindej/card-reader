using CardReader.Application;
using Microsoft.EntityFrameworkCore.Storage;

namespace CardReader.Infrastructure.Persistence;

internal class UnitOfWork : IUnitOfWork
{
    private readonly GymDoorDbContext _context;
    private IDbContextTransaction _transaction;

    public UnitOfWork(GymDoorDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            // Log the exception (logging mechanism not shown here)
            throw new Exception("An error occurred while saving changes.", ex);
        }
    }
    
    public void ClearChangeTracker()
    {
        _context.ChangeTracker.Clear();
    }

    public async Task BeginTransactionAsync()
    {
        try
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }
        catch (Exception ex)
        {
            // Log the exception (logging mechanism not shown here)
            throw new Exception("An error occurred while beginning the transaction.", ex);
        }
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await RollbackTransactionAsync();
            // Log the exception (logging mechanism not shown here)
            throw new Exception("An error occurred while committing the transaction.", ex);
        }
        finally
        {
            await _transaction.DisposeAsync();
        }
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            await _transaction.RollbackAsync();
        }
        catch (Exception ex)
        {
            // Log the exception (logging mechanism not shown here)
            throw new Exception("An error occurred while rolling back the transaction.", ex);
        }
        finally
        {
            await _transaction.DisposeAsync();
            ClearChangeTracker();
        }
    }
}