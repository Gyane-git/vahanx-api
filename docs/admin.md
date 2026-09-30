# Admin Module

## Overview

The Admin module provides the administrative control layer over the VahanX platform. It includes dashboard summaries, permission-based authorization, and administrative APIs.

## Roles

Roles are database-driven and not hardcoded. Example roles include:

- **SuperAdmin** - Full platform access
- **Admin** - Administrative access
- **Moderator** - Content moderation access
- **ContentManager** - CMS content management
- **AnalyticsViewer** - Analytics viewing only
- **AuditViewer** - Audit log viewing only
- **SupportAgent** - Support operations

## Permissions

Permissions are database-driven and assigned to roles. Key permissions include:

| Permission | Description |
|------------|-------------|
| USER_VIEW | View users |
| USER_MANAGE | Manage users |
| USER_SUSPEND | Suspend users |
| LISTING_VIEW | View listings |
| LISTING_MODERATE | Moderate listings |
| LISTING_APPROVE | Approve listings |
| LISTING_REJECT | Reject listings |
| SELLER_VIEW | View sellers |
| SELLER_VERIFY | Verify sellers |
| SELLER_SUSPEND | Suspend sellers |
| DEALER_VIEW | View dealers |
| DEALER_VERIFY | Verify dealers |
| DEALER_SUSPEND | Suspend dealers |
| REVIEW_MODERATE | Moderate reviews |
| ADVERTISEMENT_MODERATE | Moderate advertisements |
| VERIFICATION_REVIEW | Review verifications |
| INSPECTION_REVIEW | Review inspections |
| CMS_VIEW | View CMS content |
| CMS_MANAGE | Manage CMS content |
| ANALYTICS_VIEW | View analytics |
| ANALYTICS_REVENUE_VIEW | View revenue analytics |
| ANALYTICS_USER_VIEW | View user analytics |
| ANALYTICS_AD_VIEW | View advertisement analytics |
| AUDIT_VIEW | View audit logs |
| SYSTEM_CONFIG_VIEW | View system configuration |
| SYSTEM_CONFIG_MANAGE | Manage system configuration |

## Dashboard API

### GET /api/v1/admin/dashboard/summary

Returns high-level platform metrics:

- Total users, active users, new users
- Total listings, published listings, pending listings, sold listings
- Total sellers, dealers
- Pending verifications, inspections, reports, moderation cases
- Active advertisements, subscriptions
- Service bookings, sell requests
- Revenue summary

## Authorization

All admin endpoints require appropriate permissions. The system uses permission-based authorization, not role-based. Permissions are verified server-side.

## Security

- Never trust user IDs from the frontend for authorization
- Derive actor identity from authenticated claims
- Verify permissions server-side
- Never allow normal users to call admin endpoints
