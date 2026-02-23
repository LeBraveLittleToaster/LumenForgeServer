# Authentication and Authorization

## Authentication

Authentication is mainly handled by keycloak, managing tokens and session.

## Authorization

Authorization is controlled by the application context and bound to the keycloak userId.

Is is divided into modules:

- Inventory
- Maintenance
- Rentals
- Billing

There are two types of roles. The first role is the external, worker, administrator or owner role (managed by keycloak REALM_ roles). The external is someone that creates rental requests, but is not allowed to see inside the system. The worker is an account type that can be assigned to a group. A group provides to scope to use functionality in the system (e.g. manage rentals). The administrator is able to manage groups and therefore handle access to the application. The owner is an administrator with the only added functionality of being able to promote and demote workers to and from administrators. Owners can demote themselves but cannot demote other owners.

Groups can be created by administrators (and owners). Groups are linked to the usersID, assigned via the administrator.


By default, only external users have content directly associated with them. While workers can be assigned to e.g. rentals for handling, they do not own the entity, even if they created it. Due to the targeted audience of small and medium size companies, we consider it sufficient that users can see all the rentals and inventory as a tradeoff for an easier implementation of access rights.

In the following, available rights per module are listed. The rights are implicitly create/read/write/delete (e.g. Device translates to Device:create, Device:read, Device:write, Device:delete).

### Inventory

Inventory is centered around the following entities or resource.

- Device
- Vendor
- Category
- Stock
≤
### Maintenance

- Backlog


### Rentals

- Rental
- RentalStatus

### Billing

- Invoice
- InvoiceStatus

## Example of a group

**Operations / Frontline Staff User**


- Can view inventory data but cannot modify it.

- Can create and manage operational records (maintenance backlog and rentals).

- Cannot delete operational records (controlled action).

- Has no access to billing (financial segregation of duties).

Cannot manage system-level entities like status definitions.

| Module          | Resource      | Create | Read | Update | Delete |
| --------------- | ------------- | :----: | :--: | :----: | :----: |
| **Inventory**   | Device        |    X   |   ✓  |    X   |    X   |
|                 | Vendor        |    X   |   ✓  |    X   |    X   |
|                 | Category      |    X   |   ✓  |    X   |    X   |
|                 | Stock         |    X   |   ✓  |    X   |    X   |
| **Maintenance** | Backlog       |    ✓   |   ✓  |    ✓   |    X   |
| **Rentals**     | Rental        |    ✓   |   ✓  |    ✓   |    X   |
|                 | RentalStatus  |    -   |   ✓  |    X   |    X   |
| **Billing**     | Invoice       |    X   |   X  |    X   |    X   |
|                 | InvoiceStatus |    X   |   X  |    X   |    X   |


**Finance / Billing User**


- Full control over billing records (invoices).
- Can update financial statuses (e.g., paid, voided).
- Cannot delete invoices (audit protection).
- Read-only visibility into rentals (for reconciliation).
- No access to inventory configuration.
- No access to maintenance workflows.
- Cannot modify operational status definitions outside billing.

| Module          | Resource      | Create | Read | Update | Delete |
| --------------- | ------------- | :----: | :--: | :----: | :----: |
| **Inventory**   | Device        |    X   |   ✓  |    X   |    X   |
|                 | Vendor        |    X   |   ✓  |    X   |    X   |
|                 | Category      |    X   |   ✓  |    X   |    X   |
|                 | Stock         |    X   |   ✓  |    X   |    X   |
| **Maintenance** | Backlog       |    X   |   ✓  |    X   |    X   |
| **Rentals**     | Rental        |    X   |   ✓  |    X   |    X   |
|                 | RentalStatus  |    -   |   ✓  |    X   |    X   |
| **Billing**     | Invoice       |    ✓   |   ✓  |    ✓   |    X   |
|                 | InvoiceStatus |    -   |   ✓  |    ✓   |    X   |
