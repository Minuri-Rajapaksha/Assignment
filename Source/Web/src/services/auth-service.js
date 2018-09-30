"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var oidc_client_1 = require("oidc-client");
var http_1 = require("@angular/http");
var AuthService = /** @class */ (function () {
    function AuthService() {
        var _this = this;
        this.user = null;
        this.manager = new oidc_client_1.UserManager(this.getClientSettings());
        this.manager.getUser().then(function (user) {
            _this.user = user;
        });
    }
    AuthService.prototype.getClientSettings = function () {
        return {
            authority: "https://localhost:5000",
            client_id: "granTypt-implicit_clientSide",
            redirect_uri: "https://localhost:8081/auth-callback",
            post_logout_redirect_uri: "https://localhost:8081/",
            response_type: "id_token token",
            scope: "openid profile implicit_api.full_access",
            filterProtocolClaims: true,
            loadUserInfo: true
        };
    };
    AuthService.prototype.isLoggedIn = function () {
        return this.user != null && !this.user.expired;
    };
    AuthService.prototype.getClaims = function () {
        return this.user.profile;
    };
    AuthService.prototype.getAuthorizationHeaderValue = function () {
        return new http_1.RequestOptions({
            headers: new http_1.Headers({ 'Authorization': this.user.token_type + " " + this.user.access_token })
        });
    };
    AuthService.prototype.startAuthentication = function () {
        return this.manager.signinRedirect();
    };
    AuthService.prototype.completeAuthentication = function () {
        var _this = this;
        return this.manager.signinRedirectCallback().then(function (user) {
            _this.user = user;
        });
    };
    return AuthService;
}());
exports.AuthService = AuthService;
//# sourceMappingURL=auth-service.js.map