using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models;

namespace Storage
{
    public interface IDatabaseStorage<T>
    {
        Task<T> GetSingleAsync(string whereClause, object parameters, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<T>> GetListAsync(string whereClause, object parameters, CancellationToken cancellationToken);
        Task AddAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(string whereClause, object parameters, CancellationToken cancellationToken);
        Task UpdateAsync(string whereClause, object parameters, T updatedEntity, CancellationToken cancellationToken);
    }
}
// using System;
// using System.Collections.Generic;
// using System.Threading;
// using System.Threading.Tasks;
//
// namespace Storage
// {
//     public interface IDatabaseStorage<T>
//     {
//        
//         Task<T> GetSingleAsync(string whereClause, object parameters, CancellationToken cancellationToken);
//
//        
//         Task<IReadOnlyCollection<T>> GetListAsync(string whereClause, object parameters, CancellationToken cancellationToken);
//
//         
//         Task AddAsync(T entity, CancellationToken cancellationToken);
//
//         
//         Task DeleteAsync(string whereClause, object parameters, CancellationToken cancellationToken);
//
//         
//         Task UpdateAsync(string whereClause, object parameters, T updatedEntity, CancellationToken cancellationToken);
//     }
// }
