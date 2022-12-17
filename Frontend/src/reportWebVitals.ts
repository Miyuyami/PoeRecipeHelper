import { ReportHandler } from "web-vitals";

const reportWebVitals = (onPerfEntry?: ReportHandler): void => {
  if (onPerfEntry != null) {
    import("web-vitals")
      .then(({ getCLS, getFID, getFCP, getLCP, onTTFB }) => {
        getCLS(onPerfEntry);
        getFID(onPerfEntry);
        getFCP(onPerfEntry);
        getLCP(onPerfEntry);
        onTTFB(onPerfEntry);
      })
      .catch((error) => {
        // eslint-disable-next-line no-console
        console.error(error);
      });
  }
};

export default reportWebVitals;
