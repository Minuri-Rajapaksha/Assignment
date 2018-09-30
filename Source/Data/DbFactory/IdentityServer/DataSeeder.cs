using Shared.Extensions;
using Shared.Model.DB.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DbFactory.IdentityServer
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IdentityDbFactory identityDbFactory)
        {
            using (var uow = await identityDbFactory.BeginUnitOfWorkAsync())
            {
                var clientImplicitFlow = uow.Clients.Get().FirstOrDefault(c => c.ClientId == "grantTypeImplicit");
                if (clientImplicitFlow == null)
                {
                    clientImplicitFlow = new Clients
                    {
                        ClientId = "grantTypeImplicit",
                        ClientName = "angular web client",
                        AllowRememberConsent = true,
                        Enabled = true,
                        ProtocolType = "oidc",
                        AllowAccessTokensViaBrowser = true,
                        RequireClientSecret = true,
                        AccessTokenLifetime = 30,
                        IdentityTokenLifetime = 45,
                        SlidingRefreshTokenLifetime = 60,
                        EnableLocalLogin = true,
                        IncludeJwtId = true,
                        ClientRedirectUris = new List<ClientRedirectUris>
                        {
                            new ClientRedirectUris
                            {
                                RedirectUri = "http://localhost:5896/signin-oidc",
                            }
                        },
                        ClientPostLogoutRedirectUris = new List<ClientPostLogoutRedirectUris>
                        {
                            new ClientPostLogoutRedirectUris
                            {
                                PostLogoutRedirectUri = "http://localhost:5896/signout-oidc",
                            }
                        },
                        ClientGrantTypes = new List<ClientGrantTypes>
                        {
                            new ClientGrantTypes
                            {
                                GrantType = "implicit"
                            }
                        },
                        ClientSecrets = new List<ClientSecrets>
                        {
                            new ClientSecrets
                            {
                                Type = "SharedSecret",
                                Description = "Implicit client secret",
                                Value = "2D5qMinuri@#".Sha256()
                            }
                        },
                        ClientScopes = new List<ClientScopes>
                        {
                            new ClientScopes
                            {
                                Scope = "openid"
                            },
                            new ClientScopes
                            {
                                Scope = "profile"
                            },
                             new ClientScopes
                            {
                                Scope = "webapi.full_access"
                            }
                        },
                        ClientCorsOrigins = new List<ClientCorsOrigins>
                        {
                            new ClientCorsOrigins
                            {
                                Origin = "http://localhost:5896"
                            }
                        }
                    };
                    uow.Clients.Insert(clientImplicitFlow);

                    uow.ApiResources.Insert(new ApiResources
                    {
                        Name = "webapi",
                        Description = "web api protected by identity server",
                        DisplayName = "apiresource-web api",
                        Enabled = true,
                        ApiSecrets = new List<ApiSecrets>
                        {
                            new ApiSecrets
                            {
                                Type = "SharedSecret",
                                Description = "Web api secret",
                                Value = "4h6KMinuri@#".Sha256()
                            }
                        },
                        ApiScopes = new List<ApiScopes>
                        {
                            new ApiScopes
                            {
                                Name = "webapi.full_access",
                                DisplayName = "web api full access",
                                Description = "web api full access",
                                ShowInDiscoveryDocument = false
                            }
                        }
                    });

                    uow.IdentityResources.Insert(new IdentityResources
                    {
                        Name = "openid",
                        Description = "identityResource - openid",
                        DisplayName = "identityResource - openid",
                        Enabled = true,
                        Emphasize = false,
                        Required = true,
                        ShowInDiscoveryDocument = false
                    });

                    uow.IdentityResources.Insert(new IdentityResources
                    {
                        Name = "profile",
                        Description = "identityResource - profile",
                        DisplayName = "identityResource - profile",
                        Enabled = true,
                        Emphasize = false,
                        Required = false,
                        ShowInDiscoveryDocument = false
                    });

                    await uow.SaveAsync();
                }

            }
        }
    }
}
