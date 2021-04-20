CREATE TABLE Accounts
(
    AccountId         CHAR(36)  NOT NULL PRIMARY KEY,
    Balance           INT       NOT NULL,
    TransactionsCount INT       NOT NULL,
    CreatedAt         TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt         TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Transactions
(
    TransactionId CHAR(36)  NOT NULL PRIMARY KEY,
    AccountId     CHAR(36)  NOT NULL,
    Amount        INT       NOT NULL,
    CreatedAt     TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (AccountId) REFERENCES Accounts (AccountId)
);
