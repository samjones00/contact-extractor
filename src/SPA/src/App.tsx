import { FormEvent, useState } from 'react';

type ContactResult = {
  name: string;
  address: string;
  telephone: string;
  description: string;
  url: string;
};

type ApiResponse = {
  location: string;
  count: number;
  results: ContactResult[];
};

const locationOptions = ['London', 'Birmingham', 'Leeds'];

function App() {
  const [selectedLocation, setSelectedLocation] = useState('');
  const [submittedLocation, setSubmittedLocation] = useState('');
  const [results, setResults] = useState<ContactResult[]>([]);
  const [count, setCount] = useState<number | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (!selectedLocation) {
      return;
    }

    setLoading(true);
    setError('');
    setResults([]);
    setCount(null);

    try {
      const response = await fetch('/Contacts', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ location: selectedLocation }),
      });

      if (!response.ok) {
        throw new Error(`Request failed with status ${response.status}`);
      }

      const data = (await response.json()) as ApiResponse;
      setSubmittedLocation(data.location);
      setCount(data.count);
      setResults(Array.isArray(data.results) ? data.results : []);
    } catch (fetchError) {
      if (fetchError instanceof Error) {
        setError(fetchError.message);
      } else {
        setError('An unexpected error occurred');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container py-4">
      <div className="row justify-content-center">
        <div className="col-12 col-md-10 col-lg-8">
          <div className="card shadow-sm">
            <div className="card-body">
              <h1 className="h4 mb-4">Solicitor Search</h1>
              <form onSubmit={handleSubmit} className="row g-3 align-items-end">
                <div className="col-12 col-sm-8">
                  <label htmlFor="locationSelect" className="form-label">
                    Location
                  </label>
                  <select
                    id="locationSelect"
                    className="form-select"
                    value={selectedLocation}
                    onChange={(event) => setSelectedLocation(event.target.value)}
                  >
                    <option value="">Select a city</option>
                    {locationOptions.map((location) => (
                      <option key={location} value={location}>
                        {location}
                      </option>
                    ))}
                  </select>
                </div>
                <div className="col-12 col-sm-4 d-grid">
                  <button className="btn btn-primary" type="submit" disabled={!selectedLocation || loading}>
                    {loading ? 'Searching…' : 'Search'}
                  </button>
                </div>
              </form>

              {error && (
                <div className="alert alert-danger mt-4" role="alert">
                  {error}
                </div>
              )}

              {count !== null && !error && (
                <div className="mt-4">
                  <div className="d-flex flex-column flex-sm-row justify-content-between align-items-start align-items-sm-center mb-3 gap-2">
                    <div>
                      <strong>{count}</strong> result{count === 1 ? '' : 's'} returned
                    </div>
                    <div className="text-muted">
                      Location: {submittedLocation || selectedLocation}
                    </div>
                  </div>

                  {results.length > 0 ? (
                    <div className="list-group">
                      {results.map((item, index) => (
                        <div key={index} className="list-group-item list-group-item-action mb-3">
                          <div>
                            <h5 className="mb-1">{item.name || 'Unknown firm'}</h5>
                            {item.url && <p className="mb-1"><a href={item.url} target="_blank" rel="noreferrer">{item.url}</a></p>}
                            <p className="mb-1">{item.address}</p>
                            <p className="mb-1">
                              <strong>Telephone:</strong> {item.telephone}
                            </p>
                            {item.description && <p className="mb-1">{item.description}</p>}
                          </div>
                        </div>
                      ))}
                    </div>
                  ) : (
                    <div className="alert alert-secondary" role="status">
                      No results returned.
                    </div>
                  )}
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default App;
