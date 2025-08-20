using Challenges.Data.Interfaces;
using Challenges.Interfaces; // Ensure this is present if IGenericRepository is in a different namespace
using Challenges.Models;

namespace Challenges.Interfaces
{
    // IUnitOfWork provides access to different repositories and a Save method.
    public interface IUnitOfWork : IDisposable
    {
        // Property to get the Category repository.
        IGenericRepository<Category> Categories { get; }
        
        // Property to get the Challenge repository.
        IGenericRepository<Challenge> Challenges { get; }

        // Property to get the User repository.
        IGenericRepository<User> Users { get; }

        // Property to get the Profile repository.
        IGenericRepository<Profile> Profiles { get; }

        // Property to get the Progress repository.
        IGenericRepository<Progress> Progresses { get; }

        // Property to get the UserChallenge repository (for the many-to-many relationship).
        IGenericRepository<UserChallenge> UserChallenges { get; }

        // Save all pending changes to the database asynchronously.
        Task<int> CompleteAsync();
    }
}
