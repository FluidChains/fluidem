import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";

const renderException = ex => {
  return (
    <div className="card">
      <h1 className="card-header">{ex.exceptionType}</h1>
      <div className="card-body">
        <h4>Code Status</h4>
        <samp className="text-info">{ex.statusCode}</samp>
        <h4>Message</h4>
        <samp className="text-warning">{ex.message}</samp>
        <h4>Stack Trace</h4>
        <samp className="text-danger">{ex.stackTrace}</samp>
      </div>
    </div>
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
      <div className="col-12">
        <p>Info Exception</p>
        {contents}
      </div>
    </div>
  );
};

ExceptionDetail.propTypes = {
  match: PropTypes.object,
  apiUrl: PropTypes.string,
  errorHandler: PropTypes.func,
};

export default ExceptionDetail;
