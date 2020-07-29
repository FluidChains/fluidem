import React, { useState, useEffect } from "react";

const renderException = ex =>{
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

const ExceptionDetail = () => {
  const [loading, setLoading] = useState(true);
  const [exception, setException] = useState({});

  useEffect( () =>{
    // getException(id);
  }, [] );

  const getException = async (id) => {
    const resp = await fetch(`api/exception-detail/${id}`);
    const data = await resp.json();

    setLoading(false);
    setException(data.infException[0]);
  };

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

export default ExceptionDetail;

/*
class ExceptionDetail extends React.Component {
  constructor(props) {
    super(props);
    this.state = { exception: {}, loading: true };
  }

  componentDidMount() {
    console.log(this.props);
    this.getException(this.props.match.params.id);
  }

  async getException(id) {
    const resp = await fetch(`api/exception-detail/${id}`);
    const data = await resp.json();
    this.setState({ exception: data.infException[0], loading: false });
  }

  renderException(ex) {
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
  }

  render() {
    let contents = this.state.loading ? (
      <div className="spinner-border text-danger" role="status">
        <span className="sr-only">Loading...</span>
      </div>
    ) : (
      this.renderException(this.state.exception)
    );
    return (
      <div className="row">
        <div className="col-12">
          <p>Info Exception</p>
          {contents}
        </div>
      </div>
    );
  }
}

export default ExceptionDetail;
*/