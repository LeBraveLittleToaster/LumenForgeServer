```mermaid
erDiagram
    VENDOR ||--o{ DEVICE : supplies
    CATEGORY ||--o{ DEVICE : categorizes
    DEVICE ||--|| STOCK : has
    DEVICE ||--o{ DEVICE_PARAMETER : contains
    MAINTENANCE_STATUS ||--o{ DEVICE : assigns

    VENDOR {
        long id PK
        string uuid UK
        string name UK
    }

    CATEGORY {
        long id PK
        string uuid UK
        string name
        string description
    }

    DEVICE {
        long id PK
        uuid uuid UK
        string serial_number UK
        string device_name
        string device_description
        string photo_url
        long vendor_id FK
        decimal purchase_price
        date purchase_date
        long maintenance_status_id FK
        timestamp create_date
        timestamp update_date
    }

    STOCK {
        long id PK
        uuid uuid UK
        long device_id FK "One-to-One"
        string unit_stock_type
        decimal stock_count
        boolean is_fractional
    }

    DEVICE_PARAMETER {
        long id PK
        string key
        string value
        long device_id FK
    }

    MAINTENANCE_STATUS {
        long id PK
        string uuid UK
        string name
        string description
    }

    DEVICE_CATEGORY {
        long device_id FK
        long category_id FK
    }
```
