﻿using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.Domain.Employees.Roles;
using Supermarket.Core.UseCases.Admin;

namespace Supermarket.Wpf.Admin.Employees.Dialog;

public class AdminMenuEmployeeModel : NotifyPropertyChangedBase
{
    private string? _name;
    private string? _surname;
    private string? _login;
    private DateTime? _hireDate;
    private bool _isCashier;
    private bool _isGoodsKeeper;
    private bool _isManager;
    private bool _isAdmin;
    private string? _personalNumber;
    private bool _isNotManagerRoleAllowed = true;

    public int Id { get; set; }
    
    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string? Surname
    {
        get => _surname;
        set => SetProperty(ref _surname, value);
    }

    public string? Login
    {
        get => _login;
        set => SetProperty(ref _login, value);
    }

    public DateTime? HireDate
    {
        get => _hireDate;
        set => SetProperty(ref _hireDate, value);
    }

    public bool IsCashier
    {
        get => _isCashier;
        set => SetProperty(ref _isCashier, value);
    }

    public bool IsGoodsKeeper
    {
        get => _isGoodsKeeper;
        set => SetProperty(ref _isGoodsKeeper, value);
    }
    
    public bool IsNotManagerRoleAllowed
    {
        get => _isNotManagerRoleAllowed;
        private set => SetProperty(ref _isNotManagerRoleAllowed, value);
    }

    public bool IsManager
    {
        get => _isManager;
        set
        {
            SetProperty(ref _isManager, value);
            if (value)
            {
                IsCashier = true;
                IsGoodsKeeper = true;
            }
            IsNotManagerRoleAllowed = !value;
        }
    }

    public bool IsAdmin
    {
        get => _isAdmin;
        set
        {
            SetProperty(ref _isAdmin, value);
            if (value)
            {
                IsCashier = false;
                IsGoodsKeeper = false;
                IsManager = false;
            }
            IsNotManagerRoleAllowed = !value;
        }
    }

    public string? PersonalNumber
    {
        get => _personalNumber;
        set => SetProperty(ref _personalNumber, value);
    }

    public static AdminMenuEmployeeModel FromAdminEmployeeDetail(AdminEmployeeDetail adminEmployeeDetail)
    {
        var model = new AdminMenuEmployeeModel()
        {
            Id = adminEmployeeDetail.Id,
            Name = adminEmployeeDetail.Name,
            Surname = adminEmployeeDetail.Surname,
            Login = adminEmployeeDetail.Login,
            HireDate = adminEmployeeDetail.HireDate,
            PersonalNumber = adminEmployeeDetail.PersonalNumber
        };
        
        if (adminEmployeeDetail.RoleInfo is Core.Domain.Employees.Roles.Admin)
        {
            model.IsAdmin = true;
        }
        else if (adminEmployeeDetail.RoleInfo is SupermarketEmployee supermarketEmployee)
        {
            model.IsCashier = supermarketEmployee.Roles.Contains(SupermarketEmployeeRole.Cashier);
            model.IsGoodsKeeper = supermarketEmployee.Roles.Contains(SupermarketEmployeeRole.GoodsKeeper);
            model.IsManager = supermarketEmployee.Roles.Contains(SupermarketEmployeeRole.Manager);
        }
        else
        {
            throw new UiInconsistencyException("Unknown employee role.");
        }

        return model;
    }
    
    public IEmployeeRoleInfo GetEmployeeRoleInfo(int? supermarketId, int? managerId)
    {
        if (IsAdmin)
        {
            return new Core.Domain.Employees.Roles.Admin();
        }

        if (!supermarketId.HasValue)
        {
            throw new InvalidOperationException($"{nameof(supermarketId)} is null");
        }
        return new SupermarketEmployee(supermarketId.Value, managerId, GetRoles());
    }
    
    private HashSet<SupermarketEmployeeRole> GetRoles()
    {
        var roles = new HashSet<SupermarketEmployeeRole>();
        if (IsCashier)
        {
            roles.Add(SupermarketEmployeeRole.Cashier);
        }

        if (IsGoodsKeeper)
        {
            roles.Add(SupermarketEmployeeRole.GoodsKeeper);
        }

        if (IsManager)
        {
            roles.Add(SupermarketEmployeeRole.Manager);
        }

        return roles;
    }
}