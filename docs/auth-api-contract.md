# VahanX Auth & Access-Control API Contract

Base URL (development): `http://localhost:5001`
API prefix: `/api/v1`
Swagger: `http://localhost:5001/swagger/index.html`

Authentication: `Authorization: Bearer <accessToken>` (JWT).

All protected responses may return:

- `401` — missing/invalid/expired token, invalid credentials (no account-enumeration specifics).
- `403` — authenticated but missing the required permission policy.
- `404` — resource not found.
- `409` — duplicate/system-role conflicts.

Response envelope (all endpoints):

```json
{
  "success": true,
  "message": "Request successful",
  "data": { },
  "errors": [],
  "traceId": "..."
}
```

Errors use: `{ "success": false, "message": "...", "data": null, "errors": [{ "code", "field", "message" }], "traceId": "..." }`.

## Auth

### POST /api/v1/auth/login

Request:

```json
{ "email": "admin@vahanx.com", "password": "VahanX@Dev123!" }
```

Response `200`:

```json
{
  "data": {
    "accessToken": "<jwt>",
    "refreshToken": "<opaque>",
    "accessTokenExpiresAt": "2026-10-02T06:17:00Z",
    "refreshTokenExpiresAt": "2026-10-09T05:47:00Z",
    "user": {
      "id": "guid",
      "email": "admin@vahanx.com",
      "firstName": "Super",
      "lastName": "Admin",
      "phoneNumber": "+977-9800000000",
      "isActive": true,
      "emailConfirmed": true,
      "lastLoginAt": "2026-10-02T05:47:15Z",
      "roles": ["SuperAdmin"],
      "permissions": ["USER_VIEW", "..."]
    }
  }
}
```

`401` for invalid credentials, inactive account, or suspended account.

### POST /api/v1/auth/refresh

Request: `{ "refreshToken": "<opaque>" }`

Response `200`:

```json
{
  "data": {
    "accessToken": "<jwt>",
    "refreshToken": "<new opaque token>",
    "accessTokenExpiresAt": "...",
    "refreshTokenExpiresAt": "..."
  }
}
```

Refresh tokens rotate on every use; the old token is revoked. Reusing a revoked
token revokes the whole session family (`401`).
Invalid, expired, or revoked tokens return `401`.

### POST /api/v1/auth/logout

Request: `{ "refreshToken": "<opaque>" }` → `200`, revokes the session.

### GET /api/v1/auth/me

Requires `Authorization: Bearer <token>` → `200` with the current user:

```json
{
  "data": {
    "id": "guid",
    "email": "...",
    "firstName": "...",
    "lastName": "...",
    "phoneNumber": "...",
    "isActive": true,
    "emailConfirmed": true,
    "lastLoginAt": "...",
    "roles": ["SuperAdmin"],
    "permissions": ["USER_VIEW", "..."]
  }
}
```

## Admin users (all require authentication)

| Endpoint | Permission |
|---|---|
| GET /api/v1/admin/users | USER_VIEW |
| GET /api/v1/admin/users/{id} | USER_VIEW |
| POST /api/v1/admin/users | USER_CREATE |
| PUT /api/v1/admin/users/{id} | USER_UPDATE |
| DELETE /api/v1/admin/users/{id} | USER_DELETE |
| POST /api/v1/admin/users/{id}/activate | USER_ACTIVATE |
| POST /api/v1/admin/users/{id}/deactivate | USER_DEACTIVATE |
| POST /api/v1/admin/users/{id}/suspend | USER_SUSPEND |
| GET /api/v1/admin/users/{id}/roles | USER_VIEW |
| POST /api/v1/admin/users/{id}/roles | USER_ROLE_ASSIGN |
| DELETE /api/v1/admin/users/{id}/roles/{roleId} | USER_ROLE_ASSIGN |

`GET /api/v1/admin/users` supports `page`, `pageSize`, `search`, `isActive`.

Create user request:

```json
{
  "email": "user@vahanx.com",
  "password": "Password@123",
  "firstName": "Jane",
  "lastName": "Doe",
  "phoneNumber": "+977-9810000000",
  "isActive": true,
  "emailConfirmed": true,
  "roleIds": ["<role-guid>"]
}
```

Update user request: `{ "firstName", "lastName", "phoneNumber", "emailConfirmed" }`.
Suspend user request: `{ "reason": "Violation of terms" }`.
Assign role request: `{ "roleId": "<role-guid>" }`.

Validation errors return `400` with field-level errors. Duplicate email returns
`409`. Duplicate role assignment returns `409`.

## Admin roles

| Endpoint | Permission |
|---|---|
| GET /api/v1/admin/roles | ROLE_VIEW |
| GET /api/v1/admin/roles/{id} | ROLE_VIEW |
| POST /api/v1/admin/roles | ROLE_CREATE |
| PUT /api/v1/admin/roles/{id} | ROLE_UPDATE |
| DELETE /api/v1/admin/roles/{id} | ROLE_DELETE |
| GET /api/v1/admin/roles/{id}/permissions | ROLE_VIEW |
| POST /api/v1/admin/roles/{id}/permissions | ROLE_PERMISSION_ASSIGN |
| DELETE /api/v1/admin/roles/{id}/permissions/{permissionId} | ROLE_PERMISSION_ASSIGN |

- System roles (e.g. `SuperAdmin`) cannot be edited, deleted, or have their
  permissions modified → `403`.
- Deleting a role that is assigned to users → `409`.
- Create role: `{ "name": "Moderator", "description": "...", "isActive": true }`.
- Duplicate role name → `409`. Duplicate permission assignment → `409`.

## Admin permissions (read-only, system-defined)

| Endpoint | Permission |
|---|---|
| GET /api/v1/admin/permissions | PERMISSION_VIEW |
| GET /api/v1/admin/permissions/{id} | PERMISSION_VIEW |

## Protected existing admin endpoints

| Area | Permission |
|---|---|
| GET /api/v1/admin/dashboard/summary | ANALYTICS_VIEW |
| GET /api/v1/admin/analytics/* | ANALYTICS_VIEW |
| GET /api/v1/admin/audit[/*] | AUDIT_VIEW |
| GET /api/v1/admin/cms (reads, GET) | CMS_VIEW |
| POST/PUT/DELETE /api/v1/admin/cms/* | CMS_MANAGE |
| GET /api/v1/admin/moderation reports/cases/listings (GET) | REPORT_VIEW |
| Moderation writes (reports/cases resolve, listing approve/reject/hide/restore) | REPORT_MANAGE / LISTING_MODERATE / LISTING_REJECT |
| POST /api/v1/admin/moderation/users/{id}/restrict | USER_SUSPEND |
| POST /api/v1/admin/moderation/users/restrictions/{id}/deactivate | USER_SUSPEND |

## Pagination

`GET /api/v1/admin/users` returns:

```json
{
  "data": {
    "items": [ ... ],
    "pageNumber": 1,
    "pageSize": 20,
    "totalCount": 10,
    "totalPages": 1,
    "hasPreviousPage": false,
    "hasNextPage": false
  }
}
```

## Audit & login history

Login, logout, refresh, login failures, and blocked-account attempts are stored in
`LoginHistories`. Admin actions (user/role/permission changes, suspensions) are
written to the standard `AuditLogs` (visible via `GET /api/v1/admin/audit`,
permission `AUDIT_VIEW`). Passwords, raw refresh tokens, and access tokens are
never persisted or logged.
