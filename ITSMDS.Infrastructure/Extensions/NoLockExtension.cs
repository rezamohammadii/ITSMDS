using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace ITSMDS.Infrastructure.Extensions;

public static class NoLockExtension
{
    public static async Task<List<T>> NoLockToListAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default)
    {
        using var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadUncommitted
        }, TransactionScopeAsyncFlowOption.Enabled);
        List<T> result = await query.ToListAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        scope.Complete();
        return result;
    }

    public static async Task<T?> NoLockFirstOrDefaultAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        using var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadUncommitted
        }, TransactionScopeAsyncFlowOption.Enabled);
        T result = await query.FirstOrDefaultAsync(predicate, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        scope.Complete();
        return result;
    }

    public static async Task<T?> NoLockFirstOrDefaultAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default)
    {
        using TransactionScope scope = new(TransactionScopeOption.Required, new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadUncommitted
        }, TransactionScopeAsyncFlowOption.Enabled);
        T result = await query.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        scope.Complete();
        return result;
    }

    public static async Task<bool> NoLockAnyAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default)
    {
        using TransactionScope scope = new(TransactionScopeOption.Required, new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadUncommitted
        }, TransactionScopeAsyncFlowOption.Enabled);
        bool result = await query.AnyAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        scope.Complete();
        return result;
    }

    public static async Task<bool> NoLockAnyAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        using TransactionScope scope = new(TransactionScopeOption.Required, new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadUncommitted
        }, TransactionScopeAsyncFlowOption.Enabled);
        bool result = await query.AnyAsync(predicate, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        scope.Complete();
        return result;
    }

    public static async Task<T?> NoLockMaxAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default)
    {
        using var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadUncommitted
        }, TransactionScopeAsyncFlowOption.Enabled);
        T result = await query.MaxAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        scope.Complete();
        return result;
    }

    public static async Task<bool> NoLockMaxAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        using var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadUncommitted
        }, TransactionScopeAsyncFlowOption.Enabled);
        bool result = await query.MaxAsync(predicate, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        scope.Complete();
        return result;
    }
}
