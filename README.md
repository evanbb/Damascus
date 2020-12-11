# Damascus

For creating exceptionally resilient .NET applications.

## Overview

This collection of class libraries implement common coding conventions in a slightly-opinionated way. Nested READMEs will provide more relavent details, but here is a quick overview of each lib and what's inside:

### Damascus.Core

Not entirely sure what I want here yet, but probably common extension methods and utilities that are useful for FP-flavored OOP, null-safe declarative operations, etc.

### Damascus.Domain.Abstractions

Base abstractions for building applications using common domain-driven design (DDD) concepts. These libs have been implemented a zillion different ways, so this is just my take on it. It is making some assumptions about the architecture of your system, which is detailed in the project's README.

### Damascus.Persistence.Abstractions

Base abstractions to pair with `Damascus.Domain.Abstractions` for retrieving and persisting data. Multiple strategies are legit, but tradeoffs abound. Check out the project's README for more details.
