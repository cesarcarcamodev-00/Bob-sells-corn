import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export function LoginPage() {
  const [name, setName] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const trimmed = name.trim();
    if (!trimmed) return;

    setError('');
    setLoading(true);

    try {
      await login(trimmed);
      navigate('/dashboard');
    } catch {
      setError('Login failed');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100">
      <form onSubmit={handleSubmit} className="bg-white p-8 rounded-lg shadow-sm w-80">
        <h1 className="text-xl font-bold mb-6 text-center">Corn Portal</h1>
        <input
          type="text"
          maxLength={50}
          value={name}
          onChange={(e) => setName(e.target.value)}
          placeholder="Your name"
          disabled={loading}
          className="w-full p-3 border rounded mb-4"
        />
        {error && <p className="text-red-500 text-sm mb-4">{error}</p>}
        <button
          type="submit"
          disabled={loading || !name.trim()}
          className="w-full p-3 bg-black text-white rounded disabled:opacity-50">
          {loading ? '...' : 'Enter'}
        </button>
      </form>
    </div>
  );
}
