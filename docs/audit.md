# Audit Module

## Overview

The Audit module provides business/security history tracking. It is different from technical logging (Serilog).

## What is Audited

Important changes to:
- Users
- Roles
- Permissions
- Listings
- Sellers
- Dealers
- Verification
- Inspection
- Reviews
- Service Centers
- Service Bookings
- Charging Stations
- Fuel Stations
- Subscriptions
- Advertisements
- Payments
- Refunds
- Sell Requests
- CMS content
- System configuration

## Audit Action Types

- Create
- Update
- Delete
- Approve
- Reject
- Publish
- Unpublish
- Suspend
- Unsuspend
- PasswordChanged
- RoleChanged
- PermissionChanged
- PaymentCreated
- PaymentSucceeded
- RefundCreated
- SubscriptionActivated
- SubscriptionCancelled
- ModerationAction
- VerificationAction
- CMSPublished
- CMSUnpublished

## Audit Log Fields

| Field | Description |
|-------|-------------|
| ActorUserId | Who performed the action |
| Action | What action was performed |
| EntityType | Type of entity affected |
| EntityId | ID of entity affected |
| OldValues | Previous values |
| NewValues | New values |
| Changes | Summary of changes |
| IpAddress | IP address of actor |
| UserAgent | User agent of actor |
| CorrelationId | Correlation ID for tracing |
| Timestamp | When the action occurred |
| Result | Success, Failure, or Denied |
| FailureReason | Reason for failure (if applicable) |

## Append-Only Design

Audit logs are append-only:
- Never update audit logs
- Never delete audit logs
- Never modify timestamps
- Never modify actors
- Only system retention mechanisms may archive audit records later

## Security

- Only users with AUDIT_VIEW permission can access audit logs
- Financial audit records may require stronger permissions
- Sensitive information is excluded:
  - No passwords
  - No tokens
  - No CVV
  - No sensitive credentials

## APIs

- GET /api/v1/admin/audit
- GET /api/v1/admin/audit/{id}

## Query Parameters

- actorUserId: Filter by actor
- entityType: Filter by entity type
- entityId: Filter by entity ID
- action: Filter by action
- from: Start date
- to: End date
- result: Filter by result

## Retention

Audit retention is separate from analytics retention. Destructive automatic deletion is NOT implemented in Phase 9. The system is prepared for future archival.
