# Analytics Module

## Overview

The Analytics module measures platform activity and business performance. It provides insights into user behavior, listing performance, revenue, and platform health.

## Metric Definitions

### Listing Views
Number of recorded listing-view events.

### Unique Listing Views
Distinct user/session views according to available tracking data.

### CTR (Click-Through Rate)
Clicks / Impressions x 100

### Net Revenue
Successful revenue minus successful refunds.

### Active Subscription
Subscription whose status and dates indicate current validity.

## Analytics Areas

### User Analytics
- New users
- Active users
- User growth over time

### Listing Analytics
- New listings
- Published listings
- Sold listings
- Views, unique views
- Wishlist adds
- Enquiries

### Vehicle Analytics
- Vehicle views
- Listing count
- Wishlist count
- Enquiry count
- Test drive count
- Average listing price

### Search Analytics
- Most searched brands
- Most searched models
- Most searched locations
- Searches with zero results

### Enquiry Analytics
- Enquiries created
- Enquiries responded
- Response time
- Closed enquiries

### Test Drive Analytics
- Requested
- Confirmed
- Completed
- Cancelled
- No-show
- Conversion rate

### Service Analytics
- Service bookings
- Completed bookings
- Cancelled bookings
- Popular services
- Average booking value

### Advertisement Analytics
- Impressions
- Clicks
- CTR
- Campaign count
- Active campaigns
- Spend
- Budget utilization

### Subscription Analytics
- Active subscriptions
- New subscriptions
- Renewals
- Cancellations
- Expired subscriptions
- Plan distribution
- Subscription revenue

### Sell Vehicle Analytics
- Requests submitted
- Requests completed
- Requests cancelled
- Inspections completed
- Offers created
- Offers accepted
- Average offer amount

### Revenue Analytics
- Gross revenue
- Refunds
- Net revenue
- Subscription revenue
- Advertisement revenue
- Listing promotion revenue
- Other revenue

### Platform Analytics
- New users
- Active users
- Listings
- Enquiries
- Service bookings
- Advertisements
- Revenue
- Subscriptions
- Sell requests

## Date Ranges

Support for:
- Today
- Yesterday
- Last 7 days
- Last 30 days
- This month
- Previous month
- Custom date range

All date calculations are timezone-aware. Timestamps are stored in UTC.

## Revenue Calculations

Revenue analytics reads from Payment, PaymentTransaction, PaymentRefund, and Invoice records.

Only successful/valid financial states contribute to revenue calculations:
- Failed payments are NOT counted
- Cancelled payments are NOT counted
- Refunds are handled correctly

## APIs

- GET /api/v1/admin/analytics/overview
- GET /api/v1/admin/analytics/users
- GET /api/v1/admin/analytics/listings
- GET /api/v1/admin/analytics/vehicles
- GET /api/v1/admin/analytics/search
- GET /api/v1/admin/analytics/enquiries
- GET /api/v1/admin/analytics/test-drives
- GET /api/v1/admin/analytics/services
- GET /api/v1/admin/analytics/advertisements
- GET /api/v1/admin/analytics/subscriptions
- GET /api/v1/admin/analytics/sell-vehicles
- GET /api/v1/admin/analytics/revenue
- GET /api/v1/admin/analytics/platform

## Query Parameters

- from: Start date
- to: End date
- period: Predefined period
- groupBy: Grouping option (day, week, month, year)

## Retention

Analytics data retention is configurable. Historical data is not automatically deleted unless explicitly configured.
