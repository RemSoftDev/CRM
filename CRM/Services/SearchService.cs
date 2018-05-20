using CRM.Models;
using CRM.DAL.Contexts;
using System.Collections.Generic;
using System.Linq;
using CRM.DAL.Adapters;
using AutoMapper;
using CRM.DAL.Entities;
using CRM.Interfaces;
using System.Data.Entity;

namespace CRM.Services
{
    public sealed class SearchService<TEntity> where TEntity : IUser
    {
        private readonly string GridType;
        public SearchService()
        {
            if(typeof(TEntity) == typeof(LeadViewModel))
            {
                this.GridType = "Lead";
            }
            else if(typeof(TEntity) == typeof(UserViewModel))
            {
                this.GridType = "User";
            }
        }
        public SearchViewModel FillLeadModelByProfile(GridProfileViewModel profile)
        {
            var leadAdapter = new LeadAdapter();
            var model = new SearchViewModel();

            model.Columns = profile?.GridFields ?? new List<GridFieldViewModel>();
            model.Field = profile?.SearchField;
            model.SearchValue = profile?.SearchValue;

            int totalItems;

            var result = leadAdapter.GetLeadsByFilter(
                model.Field,
                model.SearchValue,
                model.OrderField,
                model.Page,
                model.ItemsPerPage,
                out totalItems,
                model.OrderDirection);

            var items = Mapper.Map<List<Lead>, List<LeadViewModel>>(result);
            model.Items = items.ToList<IUser>();
            model.ItemsCount = totalItems;

            if (!model.Columns.Any())
            {
                var columns = ReflectionService.GetModelProperties<LeadViewModel>();
                for (int i = 0; i < columns.Count; i++)
                {
                    model.Columns.Add(new GridFieldViewModel(columns[i], true, 0, i));
                }
            }

            return model;
        }

        public SearchViewModel FillUserModelByProfile(GridProfileViewModel profile)
        {
            var userAdapter = new UserAdapter();
            var model = new SearchViewModel();

            model.Columns = profile?.GridFields ?? new List<GridFieldViewModel>();
            model.Field = profile?.SearchField;
            model.SearchValue = profile?.SearchValue;

            int totalItems;

            var result = userAdapter.GetUsersByFilter(
                model.Field,
                model.SearchValue,
                model.OrderField,
                model.Page,
                model.ItemsPerPage,
                out totalItems,
                model.OrderDirection);

            var items = Mapper.Map<List<User>, List<UserViewModel>>(result);
            model.Items = items.ToList<IUser>();
            model.ItemsCount = totalItems;

            if (!model.Columns.Any())
            {
                var columns = ReflectionService.GetModelProperties<UserViewModel>();
                for (int i = 0; i < columns.Count; i++)
                {
                    model.Columns.Add(new GridFieldViewModel(columns[i], true, 0, i));
                }
            }

            return model;
        }

        public SearchViewModel GetSearchModel(string userEmail)
        {
            var model = new SearchViewModel();
            var profiles = Mapper.Map<List<GridProfileViewModel>>(
                GetUserProfiles(userEmail)
            );

            var selectedProfile = profiles.FirstOrDefault(p => p.IsDefault);
            if(GridType == "Lead")
            {
                model = FillLeadModelByProfile(selectedProfile);
            }
            else if (GridType == "User")
            {
                model = FillUserModelByProfile(selectedProfile);
            }
            
            model.Profiles = profiles;

            return model;
        }

        public List<GridProfile> GetUserProfiles(string userEmail, string profileName = "")
        {
            using (var context = new BaseContext())
            {
                var query = context
                   .GridProfiles
                   .Include(p => p.GridFields)
                   .Where(p => p
                        .DGrid
                            .Type
                            .Equals(GridType) &&
                        p.User
                            .Email
                            .Equals(userEmail));

                if (!string.IsNullOrEmpty(profileName))
                {
                    query = query
                        .Where(p => p
                            .ProfileName
                            .Equals(profileName));
                }

                return query.OrderBy(i => i.Id).ToList();
            }
        }

        public bool EditProfile(SearchViewModel model, bool makeDefault, string currentUserEmail)
        {
            using (var context = new BaseContext())
            {
                var userId = context.Users
                    .Include(u => u.GridProfiles.Select(g => g.DGrid))
                    .FirstOrDefault(u => u.Email
                        .Equals(currentUserEmail)).Id;

                var selectedProfile = model.Profiles.FirstOrDefault(p => p.IsDefault);

                var profile = context
                    .GridProfiles
                    .Include(f => f.GridFields)
                    .Where(g =>
                        g.ProfileName
                            .Equals(selectedProfile.ProfileName) &&
                        g.UserId
                            .Equals(userId) &&
                        g.DGrid.Type
                            .Equals(GridType))
                    .FirstOrDefault();

                if (profile != null)
                {
                    var receivedFields = Mapper.Map<List<GridField>>(model.Columns);
                    var fields = profile.GridFields;

                    for (int i = 0; i < fields.Count; i++)
                    {
                        fields[i].ColumnName = receivedFields[i].ColumnName;
                        fields[i].GridOrderDirection = receivedFields[i].GridOrderDirection;
                        fields[i].IsActive = receivedFields[i].IsActive;
                        fields[i].Order = receivedFields[i].Order;
                    }

                    if (makeDefault)
                    {
                        context
                            .GridProfiles
                            .Where(g =>
                                g.UserId.Equals(userId) &&
                                g.DGrid.Type.Equals(GridType))
                            .ForEachAsync(i => i.IsDefault = false)
                            .Wait();

                        profile.IsDefault = makeDefault;
                    }

                    profile.SearchField = model.Field;
                    profile.SearchValue = model.SearchValue;

                    context.SaveChanges();

                    return true;
                }              
            }

            return false;
        }

        public bool CreateProfile(SearchViewModel model, string profileName, string currentUserEmail)
        {
            using (var context = new BaseContext())
            {
                var user = context.Users
                    .Include(u => u.GridProfiles.Select(g => g.DGrid))
                    .FirstOrDefault(u => u.Email
                        .Equals(currentUserEmail));

                var profiles = context
                    .GridProfiles
                    .Where(g =>
                        g.ProfileName
                            .Equals(profileName) &&
                        g.UserId
                            .Equals(user.Id) &&
                        g.DGrid.Type
                            .Equals(GridType));

                if (!profiles.Any() && user.Id > 0)
                {
                    var receivedFields = Mapper.Map<List<GridField>>(model.Columns);
                    var profile = new GridProfile
                    {
                        GridFields = receivedFields,
                        IsDefault = true,
                        SearchField = model.Field,
                        SearchValue = model.SearchValue,
                        ProfileName = profileName,
                        UserId = user.Id
                    };

                    var leadsGrid = user
                    .GridProfiles
                    .FirstOrDefault(g => g.DGrid.Type.Equals(GridType))
                    ?.DGrid;

                    if (leadsGrid != null)
                    {
                        foreach (var item in leadsGrid.GridProfiles)
                        {
                            item.IsDefault = false;
                        }

                        leadsGrid.GridProfiles.Add(profile);
                    }
                    else
                    {
                        context.DGrids.Add(new DGrid { Type = GridType, GridProfiles = new List<GridProfile> { profile } });
                    }

                    context.SaveChanges();

                    return true;
                }
            }

            return false;
        }
    }
}