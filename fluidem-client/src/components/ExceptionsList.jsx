import React from "react";
import { Link } from "react-router-dom";

class ExceptionsList extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      listExceptions: [],
      loading: true,
      page: 1,
    };
  }

  componentDidMount() {
    this.getExceptions();
  }

  async getExceptions() {
    const resp = await fetch(`api/list-exceptions?page=${this.state.page}`);
    const data = await resp.json();
    this.setState({
      listExceptions: data.infExceptions,
      loading: false,
    });
  }

  // renderPages(numpages){
  //    const pager = new Array(numpages);
  //    let ite = 1;
  //    return(
  //       <nav aria-label="Page navigation example">
  //          <ul className="pagination">
  //             {pager.map( _ =>
  //                <li className="page-item"><span className="page-link" >{ite++}</span></li>
  //             )}
  //          </ul>
  //       </nav>
  //    )
  // }

  render() {
    let contents = this.state.loading ? (
      <div className="spinner-border text-danger" role="status">
        <span className="sr-only">Loading...</span>
      </div>
    ) : (
      this.renderTableExceptions(this.state.listExceptions)
    );
    return (
      <div className="row">
        <div className="col-12">
          <p>All Exceptions of serve</p>
          {contents}
        </div>
      </div>
    );
  }

  renderTableExceptions(list) {
    return (
      <table className="table table-sm table-striped">
        <thead>
          <tr>
            <th>Ver</th>
            <th>Host</th>
            <th>Codigo</th>
            <th>Error</th>
            <th>Message</th>
          </tr>
        </thead>
        <tbody>
          {list.map(exc => (
            <tr key={exc.id}>
              <td>
                <Link to={`detail-exception/${exc.id}`}>
                  <button className="btn btn-link">Ver</button>
                </Link>
              </td>
              <td>{exc.host}</td>
              <td>{exc.statusCode}</td>
              <td>{exc.stackTrace}</td>
              <td>{exc.message}</td>
            </tr>
          ))}
        </tbody>
      </table>
    );
  }
}

export default ExceptionsList;
