import './styles/main.scss';

function App() {
    return (
        <div className="container">
            <h1>Cash Service</h1>
            <div className="form-group">
                <label htmlFor="amount">Amount</label>
                <input type="number" id="amount" placeholder="Enter amount" />
            </div>
            <button className="btn btn--primary">Submit</button>
        </div>
    );
}

export default App;
