import React, { useState } from "react";
import { ErrorLogViewer } from "../index";

const AppErrorLogViewer = () => {
  const [errorText, setErrorText] = useState("");
  return (
    <div>
      <h1>Error viewer</h1>
      <ErrorLogViewer
        apiUrl="http://localhost:5000/api/error-log"
        errorHandler={e => {
          console.log("handling error from outside");
          setErrorText(e.message);
        }}
      />
      {errorText && <h5>Error: {errorText}</h5>}
    </div>
  );
};

export default AppErrorLogViewer;
