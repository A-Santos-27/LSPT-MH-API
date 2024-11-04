

# URL Shortener API

A URL Shortener API that leverages the `ulvis.net` service to generate shortened URLs and manages storage of original and shortened URLs within a local database. This service optimizes API calls to `ulvis.net` by checking if a URL already exists in the database before requesting a new shortened URL.

## Features

- **Create Short URLs**: Accepts a long URL from the client and returns a shortened URL.
- **Database Storage**: Stores original URLs, shortened URLs, and metadata (e.g., creation date) for efficient retrieval.
- **Reduced API Calls**: Checks if a URL already exists to avoid unnecessary requests to `ulvis.net`, minimizing costs and latency.
- **Error Handling**: Implements error handling for API call failures, such as network issues or rate limits.
- **Restrictions**: Special characters in the long URL are not supported and will result in an error.

## Key Components

1. **Controller Layer**  
   - **Endpoints**: 
     - `/shorten` (POST): Accepts a URL and returns a shortened URL.
     - `/all` (GET): Retrieves all shortened URLs stored in the database.

2. **Service Layer**
   - **Business Logic**: Handles URL shortening logic, database checking, and API calls.
   - **Flow**:
     - **URL Exists**: If the long URL exists in the database, retrieve the shortened URL from the database.
     - **URL Does Not Exist**: If not, send the long URL to `ulvis.net` to generate a shortened URL.
   - **Error Handling**: Handles HTTP errors for third-party API calls and logs failures.

3. **Database Layer**
   - **Entity**: `URL` model containing `OriginalUrl`, `ShortUrl`, and `CreatedAt`.
   - **Indexed Fields**: The original URL field is indexed for faster lookups.
   - **No Unique ID**: The original URL itself is used as the primary index.

## URL Creation Flow

1. **Client Request**: Client sends a long URL to shorten.
2. **Database Check**: Service checks if the long URL exists in the database:
   - **Exists**: Returns the existing shortened URL.
   - **Does Not Exist**: Sends the URL to `ulvis.net` to create a new short URL.
3. **Save and Return**: The newly generated short URL is saved in the database and returned to the client.

## Project Setup

1. Clone the repository:
   ```bash
   git clone <repository-url>
   ```

2. Set up dependencies:
   ```bash
   dotnet restore
   ```

3. Configure the connection string in `appsettings.json` for the database context.

4. Run the application:
   ```bash
   dotnet run
   ```

## API Usage

### POST `/shorten`

- **Request**:
  - JSON body: `{ "originalUrl": "https://example.com" }`
- **Response**:
  - JSON: `{ "shortUrl": "https://ulvis.net/abc123" }`
- **Error**: Returns an error if the URL contains special characters.

### GET `/all`

- **Response**:
  - JSON: Array of all stored shortened URLs and their original counterparts.

## Key Considerations

1. **Error Handling for API Requests**: Handles network or third-party API failures gracefully and logs errors.
2. **Database Efficiency**: Uses indexing on the original URL field to optimize lookups.
3. **URL Restrictions**: The API does not support URLs with special characters.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
```

This `README.md` provides a clear overview of your URL Shortener API, covering essential components, flow, setup instructions, and usage guidelines. Let me know if you want any additional details added!
