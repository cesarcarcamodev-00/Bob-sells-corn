import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { api } from '../services/api';

export function DashboardPage() {
  const { client, logout } = useAuth();
  const navigate = useNavigate();
  const [count, setCount] = useState<number | null>(null);
  const [loading, setLoading] = useState(false);
  const [msg, setMsg] = useState('');

  useEffect(() => {
    if (!client) {
      navigate('/login');
      return;
    }
    fetchCount();
  }, [client, navigate]);

  const fetchCount = () => {
    api.getDashboard()
      .then(data => setCount(data.totalCornPurchased))
      .catch(() => {
        logout();
        navigate('/login');
      });
  };

  const buy = async () => {
    setLoading(true);
    setMsg('');
    try {
      const res = await api.buyCorn();
      setCount(res.totalCornPurchased);
      setMsg('Purchased!');
    } catch (err: any) {
      setMsg(err.message || 'Error');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gray-100 p-8">
      <div className="max-w-sm mx-auto">
        <div className="flex justify-between items-center mb-6">
          <span className="font-medium">{client?.name}</span>
          <button onClick={() => { logout(); navigate('/login'); }} className="text-gray-500 text-sm">Logout</button>
        </div>

        <div className="bg-white p-6 rounded-lg shadow-sm text-center mb-4">
          <p className="text-gray-500 text-sm mb-2">Corn Count</p>
          <p className="text-5xl font-bold">{count ?? '-'}</p>
        </div>

        <button
          onClick={buy}
          disabled={loading}
          className="w-full p-4 bg-black text-white rounded-lg disabled:opacity-50">
          {loading ? '...' : 'Buy Corn'}
        </button>

        {msg && <p className={`text-center mt-4 text-sm ${msg === 'Purchased!' ? 'text-green-600' : 'text-red-500'}`}>{msg}</p>}
      </div>
    </div>
  );
}
