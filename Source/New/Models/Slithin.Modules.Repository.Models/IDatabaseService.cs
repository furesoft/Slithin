﻿namespace Slithin.Modules.Repository.Models;

public interface IDatabaseService : IDisposable
{
    DatabaseAccessor GetDatabase();
}
