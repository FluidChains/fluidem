import React, { Component } from "react";
import { ErrorLogViewer } from "../index";

export default class AppErrorLogViewer extends Component {
  render() {
    return (
      <div>
        <h1>Error viewer</h1>
        <ErrorLogViewer />
      </div>
    );
  }
}
