import React, { Component } from "react";
import { Route } from "react-router";
import ExceptionsList from "./ExceptionsList";
import ExceptionDetail from "./ExceptionDetail";

export default class ErrorLogViewer extends Component {
  render() {
    return (
      <div>
        <Route path="error-log-viewer" component={ExceptionsList} />
        <Route
          path="error-log-viewer/exception-detail/:uid"
          component={ExceptionDetail}
        />
      </div>
    );
  }
}
