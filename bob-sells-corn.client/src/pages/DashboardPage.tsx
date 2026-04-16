import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export function DashboardPage() {
  const { client, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div className="min-h-screen bg-neutral-50">
      <header className="bg-white border-b border-neutral-200">
        <div className="max-w-4xl mx-auto px-6 py-4 flex items-center justify-between">
          <h1 className="text-lg font-semibold text-neutral-900">Bob's Corn</h1>
          <div className="flex items-center gap-4">
            <span className="text-sm text-neutral-600">{client?.name}</span>
            <button
              onClick={handleLogout}
              className="text-sm text-neutral-500 hover:text-neutral-900 transition-colors">
              Logout
            </button>
          </div>
        </div>
      </header>

      <main className="max-w-4xl mx-auto px-6 py-12">
        <div className="bg-white rounded-xl border border-neutral-200 p-8">
          <h2 className="text-2xl font-bold text-neutral-900 mb-2">Welcome, {client?.name}</h2>
          <p className="text-neutral-500">Your client ID: {client?.id}</p>
        </div>
         <div className="bg-white rounded-xl border border-neutral-200 p-8 mt-4">
         <p> This is where I buy new corn</p>
         </div>
      </main>
    </div>
  );
}
