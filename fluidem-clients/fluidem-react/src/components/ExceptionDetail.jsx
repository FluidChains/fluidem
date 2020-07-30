import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";

const renderException = ex => {
  return (
    <React.Fragment>
      <div className="card">
        <h5 className="card-header text-info">{ex.message}</h5>
        <div className="card-body detail-body">
          <h4 className="property-name">Id</h4>
          <samp className="property-value">{ex.id}</samp>
          <br />
          <h4 className="property-name">Type</h4>
          <samp className="property-value">{ex.exceptionType}</samp>
          <br />
          <h4 className="property-name">Code Status</h4>
          <samp className="text-info property-value">{ex.statusCode}</samp>
          <h4>Stack Trace</h4>
          <pre className="text-danger">{ex.stackTrace}</pre>
        </div>
      </div>
      <h3 className="mt-3 mb-3">Server Variables</h3>
      <ul className="list-group list-group-striped">
        <li className="list-group-item">
          <b className="float-left">Name</b>
          <b className="float-right">Value</b>
        </li>
        {Object.entries(ex.detail).map(entry => (
          <li key={entry[0]} className="list-group-item">
            <b className="float-left">{entry[0]}</b>
            <span className="float-right">{entry[1]}</span>
          </li>
        ))}
      </ul>
    </React.Fragment>
  );
};

const ExceptionDetail = props => {
  const { apiUrl, errorHandler, match } = props;
  const { id } = match.params;
  const [loading, setLoading] = useState(true);
  const [exception, setException] = useState({});

  const getException = async () => {
    try {
      const resp = await fetch(`${apiUrl}/${id}`);
      const data = await resp.json();
      setException(data);
    } catch (error) {
      errorHandler(error);
    }

    setLoading(false);
  };

  useEffect(() => {
    getException();
  }, []);

  let contents = loading ? (
    <div className="spinner-border text-danger" role="status">
      <span className="sr-only">Loading...</span>
    </div>
  ) : (
    renderException(exception)
  );

  return (
    <div className="row">
      <div className="col-12">{contents}</div>
    </div>
  );
};

ExceptionDetail.propTypes = {
  match: PropTypes.object,
  apiUrl: PropTypes.string,
  errorHandler: PropTypes.func,
};

export default ExceptionDetail;
