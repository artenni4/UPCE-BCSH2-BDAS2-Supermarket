﻿namespace Supermarket.Core.Domain.Auth.LoggedEmployees;

public record LoggedAdmin(int Id, string Name, string Surname) : ILoggedEmployee;