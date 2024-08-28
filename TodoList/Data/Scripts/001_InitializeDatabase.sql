CREATE TABLE IF NOT EXISTS "Accounts" (
    "Id" SERIAL PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "Password" TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS "TodoItems" (
    "Id" SERIAL PRIMARY KEY,
    "Name" TEXT NULL,
    "Description" TEXT NULL,
    "CreatedAt" TIMESTAMP NOT NULL,
    "Deadline" TIMESTAMP NOT NULL,
    "IsDone" BOOLEAN NOT NULL,
    "UserAccountId" INTEGER NOT NULL,
    CONSTRAINT "FK_TodoItems_Accounts_UserAccountId" FOREIGN KEY ("UserAccountId") REFERENCES "Accounts" ("Id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_TodoItems_UserAccountId" ON "TodoItems" ("UserAccountId");

