import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import { useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import moment from "moment";

const renderTableExceptions = (list, baseUrl, loadExceptions) => {
  return (
    <React.Fragment>
      <p className="float-left">All exceptions of serve</p>
      <button className="btn btn-info mb-2 mt-0 float-right" onClick={loadExceptions}>Refresh</button>
      <table className="table table-sm table-striped">
        <thead>
          <tr>
            <th>Host</th>
            <th>Code</th>
            <th>Type</th>
            <th>Error</th>
            <th>User</th>
            <th>When</th>
          </tr>
        </thead>
        <tbody>
          {list.map(ex => (
            <tr key={ex.id}>
              <td>{ex.host}</td>
              <td>{ex.statusCode}</td>
              <td>{ex.exceptionType}</td>
              <td>
                <Link to={`${baseUrl}/detail/${ex.id}`}>Detail</Link> |{" "}
                {ex.message}
              </td>
              <th>{ex.user}</th>
              <td>{moment(ex.timeUtc).calendar()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </React.Fragment>
  );
};

const ExceptionsList = props => {
  const { apiUrl, errorHandler } = props;
  const [loading, setLoading] = useState(true);
  const [listExceptions, setExceptions] = useState([]);
  const location = useLocation();

  const loadExceptions = async () => {
    if (!loading) setLoading(true);
    try {
      const resp = await fetch(apiUrl);
      const data = await resp.json();
      setExceptions(data);
    } catch (error) {
      errorHandler(error);
    }
    setLoading(false);
  };

  useEffect(() => {
    loadExceptions();
  }, []);

  let contents = loading ? (
    <div>
      <div className="spinner-border text-danger" role="status">
        <span className="sr-only">Loading...</span>
      </div>
    </div>
  ) : (
    renderTableExceptions(listExceptions, location.pathname, loadExceptions)
  );
  return (
    <div className="row">
      <div className="col-12">
        {contents}
      </div>
    </div>
  );
};

ExceptionsList.propTypes = {
  apiUrl: PropTypes.string,
  errorHandler: PropTypes.func,
};

export default ExceptionsList;
